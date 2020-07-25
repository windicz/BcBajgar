
namespace Microsoft.Azure.Batch.Samples.MultiInstanceTasks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.Azure.Batch;
    using Microsoft.Azure.Batch.Common;
    using Microsoft.Azure.Batch.Auth;
    using Microsoft.Azure.Batch.Samples.Common;

    public class DeployToAzure
    {
        public static void Main(string[] args)
        {
            try
            {
                // volání asynchroní metody main
                MainAsync().Wait();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine();
                Console.WriteLine("One or more exceptions occurred.");
                Console.WriteLine();

                SampleHelpers.PrintAggregateException(ae);
            }
            finally
            {
                Console.WriteLine();
                Console.WriteLine("Sample complete, hit ENTER to exit...");
                Console.ReadLine();
            }
        }

        public static async Task MainAsync()
        {
            const string poolId = "MultiInstanceSamplePool";
            const string jobId = "MultiInstanceSampleJob";
            const string taskId = "MultiInstanceSampleTask";

            const int numberOfNodes = 5;


            //jmeno package kterou uploaduju na azure s polu s MSMpiSetup  
            const string appPackageId = "Parallel";
            const string appPackageVersion = "1.0";

            TimeSpan timeout = TimeSpan.FromMinutes(15);

            AccountSettings accountSettings = SampleHelpers.LoadAccountSettings();

            //nakonfigurované batch accounty abych se mohl připojit ke svému účtu
            BatchSharedKeyCredentials cred = new BatchSharedKeyCredentials(
                accountSettings.BatchServiceUrl,
                accountSettings.BatchAccountName,
                accountSettings.BatchAccountKey);

            using (BatchClient batchClient = BatchClient.Open(cred))
            {
                // Vytvoření fondu výpočetních uzlů a úlohu, do které přidáme úlohu s více instancemi.
                await CreatePoolAsync(batchClient, poolId, numberOfNodes, appPackageId, appPackageVersion);
                await CreateJobAsync(batchClient, jobId, poolId);

                //batch vytvoří jednu hlavní a několik dílčích úkolů
                CloudTask multiInstanceTask = new CloudTask(id: taskId,
                    commandline: $"cmd /c mpiexec.exe -c 1 -wdir %AZ_BATCH_TASK_SHARED_DIR% %AZ_BATCH_APP_PACKAGE_{appPackageId.ToUpper()}#{appPackageVersion}%\\ParallelMpiApp.exe");

                // příkaz SPMD = více samostatných procesorů současně spouští stejný program
                multiInstanceTask.MultiInstanceSettings =
                    new MultiInstanceSettings(@"cmd /c start cmd /c smpd.exe -d", numberOfNodes);

                //zadání úkolů, vytvoří se jeden primární a několik dílčích, 
                //aby odpovídaly počtu uzlů a naplánuje se jejich provedení v uzlech
                Console.WriteLine($"Adding task [{taskId}] to job [{jobId}]...");
                await batchClient.JobOperations.AddTaskAsync(jobId, multiInstanceTask);

                //verze úlohy
                CloudTask mainTask = await batchClient.JobOperations.GetTaskAsync(jobId, taskId);

                // sledování stavu úkolů,čekáme až bude úloha dokončena
                Console.WriteLine($"Awaiting task completion, timeout in {timeout}...");
                TaskStateMonitor taskStateMonitor = batchClient.Utilities.CreateTaskStateMonitor();
                await taskStateMonitor.WhenAll(new List<CloudTask> { mainTask }, TaskState.Completed, timeout);

                //aktualizace úlohy
                await mainTask.RefreshAsync();

                string stdOut = mainTask.GetNodeFile(Constants.StandardOutFileName).ReadAsString();
                string stdErr = mainTask.GetNodeFile(Constants.StandardErrorFileName).ReadAsString();

                Console.WriteLine();
                Console.WriteLine($"Main task [{mainTask.Id}] is in state [{mainTask.State}] and ran on compute node [{mainTask.ComputeNodeInformation.ComputeNodeId}]:");
                Console.WriteLine("---- stdout.txt ----");
                Console.WriteLine(stdOut);
                Console.WriteLine("---- stderr.txt ----");
                Console.WriteLine(stdErr);

                // par sekund čas aby se stačily dílčí úlohy dokončit
                TimeSpan subtaskTimeout = TimeSpan.FromSeconds(10);
                Console.WriteLine($"Main task completed, waiting {subtaskTimeout} for subtasks to complete...");
                System.Threading.Thread.Sleep(subtaskTimeout);

                Console.WriteLine();
                Console.WriteLine("---- Subtask information ----");

                //kolekce dílčích úlohů a tisk informací o každém
                IPagedEnumerable<SubtaskInformation> subtasks = mainTask.ListSubtasks();
                await subtasks.ForEachAsync(async (subtask) =>
                {
                    Console.WriteLine("subtask: " + subtask.Id);
                    Console.WriteLine("\texit code: " + subtask.ExitCode);

                    if (subtask.State == SubtaskState.Completed)
                    {
                        //získání souborů z uzlů
                        ComputeNode node =
                            await batchClient.PoolOperations.GetComputeNodeAsync(subtask.ComputeNodeInformation.PoolId,
                                                                                 subtask.ComputeNodeInformation.ComputeNodeId);

                        string outPath = subtask.ComputeNodeInformation.TaskRootDirectory + "\\" + Constants.StandardOutFileName;
                        string errPath = subtask.ComputeNodeInformation.TaskRootDirectory + "\\" + Constants.StandardErrorFileName;

                        NodeFile stdOutFile = await node.GetNodeFileAsync(outPath.Trim('\\'));
                        NodeFile stdErrFile = await node.GetNodeFileAsync(errPath.Trim('\\'));

                        stdOut = await stdOutFile.ReadAsStringAsync();
                        stdErr = await stdErrFile.ReadAsStringAsync();

                        Console.WriteLine($"\tnode: " + node.Id);
                        Console.WriteLine("\tstdout.txt: " + stdOut);
                        Console.WriteLine("\tstderr.txt: " + stdErr);
                    }
                    else
                    {
                        Console.WriteLine($"\tSubtask {subtask.Id} is in state {subtask.State}");
                    }
                });

                // vymazání zdrojů které jsme vytvořili, abychom to nemuseli dělat manuálně(fondy,úlohy)
                Console.WriteLine();
                Console.Write("Delete job? [yes] no: ");
                string response = Console.ReadLine().ToLower();
                if (response != "n" && response != "no")
                {
                    await batchClient.JobOperations.DeleteJobAsync(jobId);
                }

                Console.Write("Delete pool? [yes] no: ");
                response = Console.ReadLine().ToLower();
                if (response != "n" && response != "no")
                {
                    await batchClient.PoolOperations.DeletePoolAsync(poolId);
                }
            }
        }

        /// <summary>
        /// Vytvoření fondu výpočetních uzlů se systémem windows 2012 R2 ve službě Batch 
        /// </summary>
        /// <param name="batchClient">klient pro vytvoření fondu</param>
        /// <param name="poolId">id fondu který chceme vytvořit</param>
        /// <param name="numberOfNodes">počet výpočetních uzlů pro fond</param>
        /// <param name="appPackageId">id balíčku aplikace</param>
        /// <param name="appPackageVersion">verze balíčku</param>
        private static async Task CreatePoolAsync(BatchClient batchClient, string poolId, int numberOfNodes, string appPackageId, string appPackageVersion)
        {
            //vytvoření fondu a poolu kde můžeme upravit vlastnosti
            Console.WriteLine($"Creating pool [{poolId}]...");
            CloudPool unboundPool =
                batchClient.PoolOperations.CreatePool(
                    poolId: poolId,
                   virtualMachineSize: "standard_d2_v2",
                    //virtualMachineSize: "Standard_H8", 
                    targetDedicatedComputeNodes: numberOfNodes,
                    cloudServiceConfiguration: new CloudServiceConfiguration(osFamily: "5"));

            //spuštění komunikace mezi uzly pro ms-mpi
            unboundPool.InterComputeNodeCommunicationEnabled = true;
            //potřebné pro víceinstanční úlohy, aby každá část úlohy byla na jednom jádru v pc
            unboundPool.MaxTasksPerComputeNode = 1;

            // aplikace a verze která se bude spouštět v uzlech
            unboundPool.ApplicationPackageReferences = new List<ApplicationPackageReference>
            {
                new ApplicationPackageReference
                {
                    ApplicationId = appPackageId,
                    Version = appPackageVersion
                }
            };

            //instalace ms-mpi na uzly
            StartTask startTask = new StartTask
            {
                CommandLine = $"cmd /c %AZ_BATCH_APP_PACKAGE_{appPackageId.ToUpper()}#{appPackageVersion}%\\MSMpiSetup.exe -unattend -force",
                UserIdentity = new UserIdentity(new AutoUserSpecification(elevationLevel: ElevationLevel.Admin)),
                WaitForSuccess = true
            };
            unboundPool.StartTask = startTask;


            await unboundPool.CommitAsync();
        }

        /// <summary>
        /// Vytvoření úlohy v batch
        /// </summary>
        /// <param name="batchClient"></param>
        /// <param name="jobId"></param>
        /// <param name="poolId"></param>
        private static async Task CreateJobAsync(BatchClient batchClient, string jobId, string poolId)
        {
            //Vytvoření úlohy do které bude přidána úloha pro více instancí
            Console.WriteLine($"Creating job [{jobId}]...");
            CloudJob unboundJob = batchClient.JobOperations.CreateJob(jobId, new PoolInformation() { PoolId = poolId });
            await unboundJob.CommitAsync();
        }
    }
}
