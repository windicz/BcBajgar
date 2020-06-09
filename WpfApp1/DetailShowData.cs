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
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading;
using System.IO;
using System.Net;

namespace WpfApp1
{
    public partial class MainWindow
    {
        
        public void DetailDataRefresh()
        {
            
            DetailTeplotaObyvak.Content = prvniTeplota.Content;
            DetailSpotrebaObyvak.Content = prvniSpotreba.Content;
            DetailTeplotaLoznice.Content = druhaTeplota.Content;
            DetailSpotrebaLoznice.Content = druhaSpotreba.Content;
            DetailTeplotaKoupelna.Content = tretiTeplota.Content;
            DetailSpotrebaKoupelna.Content = tretiSpotreba.Content;
            DetailTeplotaKuchyn.Content = ctvrtaTeplota.Content;
            DetailSpotrebaKuchyn.Content = ctvrtaSpotreba.Content;
            using (var db = new ISDatabaseEntities())
            {
                var dny = DateTime.Now.AddDays(-7);
                var query = (from j in db.Cidlas
                             where (j.datum > dny) && j.mistnost == "Obývací pokoj"
                             select j.sviceni).Average();
                sviceniLabelStatistika.Content = query;
            }
            using (var db = new ISDatabaseEntities())
            {
                var dny = DateTime.Now.AddDays(-7);
                var query = (from j in db.Cidlas
                             where (j.datum > dny) && j.mistnost == "Ložnice"
                             select j.sviceni).Average();
                sviceniLabelStatistika2.Content = query;
            }
            using (var db = new ISDatabaseEntities())
            {
                var dny = DateTime.Now.AddDays(-7);
                var query = (from j in db.Cidlas
                             where (j.datum > dny) && j.mistnost == "Koupelna"
                             select j.sviceni).Average();
                sviceniLabelStatistika3.Content = query;
            }
            using (var db = new ISDatabaseEntities())
            {
                var dny = DateTime.Now.AddDays(-7);
                var query = (from j in db.Cidlas
                             where (j.datum > dny) && j.mistnost == "Kuchyň"
                             select j.sviceni).Average();
                sviceniLabelStatistika4.Content = query;
            }

        }
        public void DetailTemperatureRefresh()
        {
            detailObyvakLabel.Content = TopeniJeObyvak.Content;
            detailLozniceLabel.Content = TopeniJeLoznice.Content;
            detailKoupelnaLabel.Content = TopeniJeKoupelna.Content;
            detailKuchynLabel.Content = TopeniJeKuchyn.Content;
        }
       
        private void DetailLivingRoomLabelShow(object sender, RoutedEventArgs e)
        {
            detailObyvakLabel.Content = TopeniJeObyvak.Content;
        }
        private void DetailLivingRoomTemperatureShow(object sender, RoutedEventArgs e)
        {
            DetailTeplotaObyvak.Content = prvniTeplota.Content;
        }
        private void DetailConsumptionLivingRoomShow(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaObyvak.Content = prvniSpotreba.Content;
        }
        private void DetailTemperatureBedroomShow(object sender, RoutedEventArgs e)
        {
            DetailTeplotaLoznice.Content = druhaTeplota.Content;
        }
        private void DetailConsumptionBedroomShow(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaLoznice.Content = druhaSpotreba.Content;
        }
        private void DetailBedroomLabelShow(object sender, RoutedEventArgs e)
        {
            detailLozniceLabel.Content = TopeniJeLoznice.Content;
        }
        private void DetailTemperatureBathroomShow(object sender, RoutedEventArgs e)
        {
            DetailTeplotaKoupelna.Content = tretiTeplota.Content;
        }
        private void DetailConsumptionBathroomShow(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaKoupelna.Content = tretiSpotreba.Content;
        }
        private void DetailBathroomLabelShow(object sender, RoutedEventArgs e)
        {
            detailKoupelnaLabel.Content = TopeniJeKoupelna.Content;
        }
        private void DetailTemperatureKitchenShow(object sender, RoutedEventArgs e)
        {
            DetailTeplotaKuchyn.Content = ctvrtaTeplota.Content;
        }
        private void DetailConsumptionKitchenShow(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaKuchyn.Content = ctvrtaSpotreba.Content;
        }
        private void DetailKitchenLabelShow(object sender, RoutedEventArgs e)
        {
            detailKuchynLabel.Content = TopeniJeKuchyn.Content;
        }
       
    }
}
