using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls.DataVisualization.Charting;
using System.Text;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading;
using System.IO;
using System.Net;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44384/TestHub")
                .Build();
            

            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };
        }
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            comboboxGraf.Items.Add("Obývací pokoj");
            comboboxGraf.Items.Add("Ložnice");
            comboboxGraf.Items.Add("Koupelna");
            comboboxGraf.Items.Add("Kuchyň");
        }

        private async void ConnectWindow(object sender, RoutedEventArgs e)
        {
            _connection.On<string>("Connected",
                                  (connectionid) =>
                                  {

                                    //  tbMain.Text = connectionid;
                                      
                                  });

             var timer = SetTimer(SendAllData, 1000);
            //zapnutí teploty na vypnuto default
            using (ISDatabaseEntities context = new ISDatabaseEntities())
            {
                Topeni cus = new Topeni()
                {

                    datum = DateTime.Now,
                    spusteni = "vypnuto"

                };
                context.Topenis.Add(cus);
                context.SaveChanges();
            }


            _connection.On<string>("Posted",
                                   (value) =>
                                   {
                                       Dispatcher.BeginInvoke((Action)(() =>

                                       {
                                           try
                                           {
                                               string[] str = value.Split(',');
                                               prvniTeplota.Content = str[0].Substring(str[0].IndexOf(':') + 1);
                                               prvniSpotreba.Content = str[1].Substring(str[1].IndexOf(':') + 1);
                                               prvniSviceni.Content = str[2].Substring(str[2].IndexOf(':') + 1);

                                               druhaTeplota.Content = str[3].Substring(str[3].IndexOf(':') + 1);
                                               druhaSpotreba.Content = str[4].Substring(str[4].IndexOf(':') + 1);
                                               druhaSviceni.Content = str[5].Substring(str[5].IndexOf(':') + 1);

                                               tretiTeplota.Content = str[6].Substring(str[6].IndexOf(':') + 1);
                                               tretiSpotreba.Content = str[7].Substring(str[7].IndexOf(':') + 1);
                                               tretiSviceni.Content = str[8].Substring(str[8].IndexOf(':') + 1);

                                               ctvrtaTeplota.Content = str[9].Substring(str[9].IndexOf(':') + 1);
                                               ctvrtaSpotreba.Content = str[10].Substring(str[10].IndexOf(':') + 1);
                                               ctvrtaSviceni.Content = str[11].Substring(str[11].IndexOf(':') + 1);
                                               //NEZAPOMENOUT ZAPNOUT NA ULOZENI DO DATABÁZE
                                               SaveAllData();
                                               DetailDataRefresh();
                                               TemperatureMainWindowRefresh();
                                               DetailTemperatureRefresh();

                                           }
                                           catch (Exception)
                                           {


                                           }


                                       }));
                                   });
            try
            {
                await _connection.StartAsync();
                SendAllData();
                
                btnConnect.IsEnabled = false;
                //sendButton.IsEnabled = true;
                
            }
            catch (Exception ex)
            {

            }
            
        }
        public void TemperatureMainWindowRefresh()
        {
            TeplotaConvertor teplota = new TeplotaConvertor();

            if (Convert.ToInt32(prvniTeplota.Content) >= teplota.Convertor(TeplotaObyvakZadana.Text))
            {
                TopeniJeObyvak.Content = "vypnuté.";
            }
            else
            {
                TopeniJeObyvak.Content = "zapnuté.";

            }
            if (Convert.ToInt32(tretiTeplota.Content) >= teplota.Convertor(TeplotaKoupelnaZadana.Text))
            {
                TopeniJeKoupelna.Content = "vypnuté.";
            }
            else
            {
                TopeniJeKoupelna.Content = "zapnuté.";
            }
            if (Convert.ToInt32(druhaTeplota.Content) >= teplota.Convertor(TeplotaLozniceZadana.Text))
            {
                TopeniJeLoznice.Content = "vypnuté.";
            }
            else
            {
                TopeniJeLoznice.Content = "zapnuté.";

            }
            if (Convert.ToInt32(ctvrtaTeplota.Content) >= teplota.Convertor(TeplotaKuchynZadana.Text))
            {
                TopeniJeKuchyn.Content = "vypnuté.";
            }
            else
            {
                TopeniJeKuchyn.Content = "zapnuté.";

            }
        }
        public void SendAllData()
        {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44384/api/values");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            Random rnd = new Random();
            int randomValue = rnd.Next(15, 41);//max hodnota je vždy o jedno měnší
            int randomValue2 = rnd.Next(1, 11);
            int randomValue3 = rnd.Next(1, 501);
            int randomValue4 = rnd.Next(15, 41);
            int randomValue5 = rnd.Next(1, 11);
            int randomValue6 = rnd.Next(1, 501);
            int randomValue7 = rnd.Next(15, 41);
            int randomValue8 = rnd.Next(1, 11);
            int randomValue9 = rnd.Next(1, 501);
            int randomValue10 = rnd.Next(15, 41);
            int randomValue11 = rnd.Next(1, 11);
            int randomValue12 = rnd.Next(1, 501);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json =
                    "'Obyvaci pokoj teplota:" + randomValue +
                    ",Obyvaci pokoj spotřeba:" + randomValue2 +
                    ",Obyvaci pokoj doba sviceni:" + randomValue3 +
                    ",Ložnice teplota:" + randomValue4 +
                    ",Ložnice spotřeba:" + randomValue5 +
                    ",Ložnice doba sviceni:" + randomValue6 +
                    ",Koupelna teplota:" + randomValue7 +
                    ",Koupelna spotřeba:" + randomValue8 +
                    ",Koupelna doba sviceni:" + randomValue9 +
                    ",Kuchyň teplota:" + randomValue10 +
                    ",Kuchyň spotřeba:" + randomValue11 +
                    ",Kuchyň doba sviceni:" + randomValue12 +
                    "'";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
        public static System.Timers.Timer SetTimer(Action Act, int Interval)
        {
            System.Timers.Timer tmr2 = new System.Timers.Timer();
            tmr2.Elapsed += (sender, args) => Act();
            tmr2.AutoReset = true;
            tmr2.Interval = Interval;
            tmr2.Start();

            return tmr2;
        }
    }
}