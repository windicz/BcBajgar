#include<mpi.h>
#include<iostream>

int factorial(int n);
int digitSum(int n);

using namespace std;
int main(int argc, char** argv) {

	int currentNode, numberOfAllNodes;
	int sumOfMultiplication=0, startInterval, endInterval, dataFromAllNodes, dataFromAllNodes2, dataFromAllNodes3, dataFromAllNodes4, sumOfPower = 0, sumOfDigit = 0, sumOfFactorial = 0;
	double startTime = 0, endTime = 0;
	int firstInterval = 1000000000,secondInterval=10000;

	MPI_Status status;
	MPI_Init(&argc, &argv);

	MPI_Comm_size(MPI_COMM_WORLD, &numberOfAllNodes);
	MPI_Comm_rank(MPI_COMM_WORLD, &currentNode);

	MPI_Barrier(MPI_COMM_WORLD);
	startTime = MPI_Wtime();
	
	startInterval = firstInterval * currentNode / numberOfAllNodes + 1;
	endInterval = firstInterval * (currentNode + 1) / numberOfAllNodes;

	for (int i = startInterval; i <= endInterval; i = i + 1) {
		sumOfPower += pow(i, 2);
		sumOfMultiplication += i;
		sumOfDigit += digitSum(i);
	}

	startInterval = secondInterval * currentNode / numberOfAllNodes + 1;
	endInterval = secondInterval * (currentNode + 1) / numberOfAllNodes;

	for (int i = startInterval; i <= endInterval; i = i + 1) {
		sumOfFactorial += factorial(i);
	}

	if (currentNode != 0) {
		MPI_Send(&sumOfMultiplication, 1, MPI_INT, 0, 1, MPI_COMM_WORLD);
		MPI_Send(&sumOfDigit, 1, MPI_INT, 0, 1, MPI_COMM_WORLD);
		MPI_Send(&sumOfFactorial, 1, MPI_INT, 0, 1, MPI_COMM_WORLD);
		MPI_Send(&sumOfPower, 1, MPI_INT, 0, 1, MPI_COMM_WORLD);
	}

	else
		for (int j = 1; j < numberOfAllNodes; j = j + 1) {
			MPI_Recv(&dataFromAllNodes, 1, MPI_INT, j, 1, MPI_COMM_WORLD, &status);
			sumOfMultiplication = sumOfMultiplication + dataFromAllNodes;
			MPI_Recv(&dataFromAllNodes2, 1, MPI_INT, j, 1, MPI_COMM_WORLD, &status);
			sumOfFactorial = sumOfFactorial + dataFromAllNodes2;
			MPI_Recv(&dataFromAllNodes3, 1, MPI_INT, j, 1, MPI_COMM_WORLD, &status);
			sumOfDigit = sumOfDigit + dataFromAllNodes3;
			MPI_Recv(&dataFromAllNodes4, 1, MPI_INT, j, 1, MPI_COMM_WORLD, &status);
			sumOfPower = sumOfPower + dataFromAllNodes4;
		}
	MPI_Barrier(MPI_COMM_WORLD);
	endTime = MPI_Wtime();

	if (currentNode == 0) {
		
		printf("Vysledny cas je: %f\n", endTime - startTime);
	}

	MPI_Finalize();
	return 0;
}
int factorial(int n)
{
	if (n < 0)
	{
		return 0;
	}

	if (n == 0 || n == 1)
	{
		return 1;
	}
	return n * factorial(n - 1);
}

int digitSum(int n)
{
	return n == 0 ? 0 : n % 10 + digitSum(n / 10);
}



