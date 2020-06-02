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
        private void DetailLivingRoomLabelShow(object sender, RoutedEventArgs e)
        {
            detailObyvakLabel.Content = TopeniJeObyvak.Content;
        }
        private void DetailLivingRoomTemperatureShow(object sender, RoutedEventArgs e)
        {
            DetailTeplotaObyvak.Text = prvniTeplota.Text;
        }
        private void DetailConsumptionLivingRoomShow(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaObyvak.Text = prvniSpotreba.Text;
        }
        private void DetailTemperatureBedroomShow(object sender, RoutedEventArgs e)
        {
            DetailTeplotaLoznice.Text = druhaTeplota.Text;
        }
        private void DetailConsumptionBedroomShow(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaLoznice.Text = druhaSpotreba.Text;
        }
        private void DetailBedroomLabelShow(object sender, RoutedEventArgs e)
        {
            detailLozniceLabel.Content = TopeniJeLoznice.Content;
        }
        private void DetailTemperatureBathroomShow(object sender, RoutedEventArgs e)
        {
            DetailTeplotaKoupelna.Text = tretiTeplota.Text;
        }
        private void DetailConsumptionBathroomShow(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaKoupelna.Text = tretiSpotreba.Text;
        }
        private void DetailBathroomLabelShow(object sender, RoutedEventArgs e)
        {
            detailKoupelnaLabel.Content = TopeniJeKoupelna.Content;
        }
        private void DetailTemperatureKitchenShow(object sender, RoutedEventArgs e)
        {
            DetailTeplotaKuchyn.Text = ctvrtaTeplota.Text;
        }
        private void DetailConsumptionKitchenShow(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaKuchyn.Text = ctvrtaSpotreba.Text;
        }
        private void DetailKitchenLabelShow(object sender, RoutedEventArgs e)
        {
            detailKuchynLabel.Content = TopeniJeKuchyn.Content;
        }
    }
}
