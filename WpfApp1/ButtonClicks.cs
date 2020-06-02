using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Controls.DataVisualization.Charting;

namespace WpfApp1
{
    public partial class MainWindow
    {
        private HubConnection _connection;

        private async void Button_Click_Connect(object sender, RoutedEventArgs e)
        {

            _connection.On<string>("Connected",
                                   (connectionid) =>
                                   {

                                       tbMain.Text = connectionid;
                                       MessageBox.Show("API bylo úspěšně připojeno");
                                   });

            _connection.On<string>("Posted",
                                   (value) =>
                                   {
                                       Dispatcher.BeginInvoke((Action)(() =>

                                       {
                                           string[] str = value.Split(',');
                                           prvniTeplota.Text = str[0].Substring(str[0].IndexOf(':') + 1);
                                           prvniSpotreba.Text = str[1].Substring(str[1].IndexOf(':') + 1);
                                           prvniSviceni.Text = str[2].Substring(str[2].IndexOf(':') + 1);

                                           druhaTeplota.Text = str[3].Substring(str[3].IndexOf(':') + 1);
                                           druhaSpotreba.Text = str[4].Substring(str[4].IndexOf(':') + 1);
                                           druhaSviceni.Text = str[5].Substring(str[5].IndexOf(':') + 1);

                                           tretiTeplota.Text = str[6].Substring(str[6].IndexOf(':') + 1);
                                           tretiSpotreba.Text = str[7].Substring(str[7].IndexOf(':') + 1);
                                           tretiSviceni.Text = str[8].Substring(str[8].IndexOf(':') + 1);

                                           ctvrtaTeplota.Text = str[9].Substring(str[9].IndexOf(':') + 1);
                                           ctvrtaSpotreba.Text = str[10].Substring(str[10].IndexOf(':') + 1);
                                           ctvrtaSviceni.Text = str[11].Substring(str[11].IndexOf(':') + 1);

                                       }));
                                   });
            try
            {
                await _connection.StartAsync();

                btnConnect.IsEnabled = false;
                //sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {

            }
        }

        public void Button_Click_SetTemperature(object sender, RoutedEventArgs e)
        {
            try
            {
                TeplotaConvertor teplota = new TeplotaConvertor();

                if (teplota.Convertor(prvniTeplota.Text) >= teplota.Convertor(TeplotaObyvakZadana.Text))
                {
                    TopeniJeObyvak.Content = "vypnuté.";
                }
                else
                {
                    TopeniJeObyvak.Content = "zapnuté.";
                    Database_TemperatureOn();
                }
                if (teplota.Convertor(tretiTeplota.Text) >= teplota.Convertor(TeplotaKoupelnaZadana.Text))
                {
                    TopeniJeKoupelna.Content = "vypnuté.";
                }
                else
                {
                    TopeniJeKoupelna.Content = "zapnuté.";
                    Database_TemperatureOn();
                }
                if (teplota.Convertor(druhaTeplota.Text) >= teplota.Convertor(TeplotaLozniceZadana.Text))
                {
                    TopeniJeLoznice.Content = "vypnuté.";
                }
                else
                {
                    TopeniJeLoznice.Content = "zapnuté.";
                    Database_TemperatureOn();
                }
                if (teplota.Convertor(ctvrtaTeplota.Text) >= teplota.Convertor(TeplotaKuchynZadana.Text))
                {
                    TopeniJeKuchyn.Content = "vypnuté.";
                }
                else
                {
                    TopeniJeKuchyn.Content = "zapnuté.";
                    Database_TemperatureOn();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("musíš zadat jen číselnou hodnotu teploty");
            }

            Statistics_UpdateHeating();

        }
        private void Button_Click_LivingRoom(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Int32.Parse(prvniTeplota.Text) >= Int32.Parse(sliderTextboxObyvak.Text))
                {
                    detailObyvakLabel.Content = "vypnuté.";
                    TopeniJeObyvak.Content = "vypnuté.";
                }
                else
                {
                    detailObyvakLabel.Content = "zapnuté.";
                    TopeniJeObyvak.Content = "zapnuté.";
                    Database_TemperatureOn();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Musíte zadat číselnou hodnotu");
            }
            
        }
        private void Button_Click_Bedroom(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Int32.Parse(druhaTeplota.Text) >= Int32.Parse(sliderTextboxLoznice.Text))
                {
                    detailLozniceLabel.Content = "vypnuté.";
                    TopeniJeLoznice.Content = "vypnuté.";
                }
                else
                {
                    detailLozniceLabel.Content = "zapnuté.";
                    TopeniJeLoznice.Content = "zapnuté.";
                    Database_TemperatureOn();
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Musíte zadat číselnou hodnotu");
            }

        }
        private void Button_Click_Bathroom(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Int32.Parse(tretiTeplota.Text) >= Int32.Parse(sliderTextboxKoupelna.Text))
                {
                    detailKoupelnaLabel.Content = "vypnuté.";
                    TopeniJeKoupelna.Content = "vypnuté.";
                }
                else
                {
                    detailKoupelnaLabel.Content = "zapnuté.";
                    TopeniJeKoupelna.Content = "zapnuté.";
                    Database_TemperatureOn();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Musíte zadat číselnou hodnotu");
            }

        }
        private void Button_Click_Kitchen(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Int32.Parse(ctvrtaTeplota.Text) >= Int32.Parse(sliderTextboxKuchyn.Text))
                {
                    detailKuchynLabel.Content = "vypnuté.";
                    TopeniJeKuchyn.Content = "vypnuté.";
                }
                else
                {
                    detailKuchynLabel.Content = "zapnuté.";
                    TopeniJeKuchyn.Content = "zapnuté.";
                    Database_TemperatureOn();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Musíte zadat číselnou hodnotu");
            }

        }


    }
}
