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
        private void LightingLabelStatisticsLivingRoom(object sender, RoutedEventArgs e)
        {
            using (var db = new ISDatabaseEntities())
            {
                var dny = DateTime.Now.AddDays(-7);
                var query = (from j in db.Cidlas
                             where (j.datum > dny) && j.mistnost == "Obývací pokoj"
                             select j.sviceni).Average();
                sviceniLabelStatistika.Content = query;
            }
        }

        private void LightingLabelStatisticsBedroom(object sender, RoutedEventArgs e)
        {
            using (var db = new ISDatabaseEntities())
            {
                var dny = DateTime.Now.AddDays(-7);
                var query = (from j in db.Cidlas
                             where (j.datum > dny) && j.mistnost == "Ložnice"
                             select j.sviceni).Average();
                sviceniLabelStatistika2.Content = query;
            }
        }


        private void LightingLabelStatisticsBathroom(object sender, RoutedEventArgs e)
        {
            using (var db = new ISDatabaseEntities())
            {
                var dny = DateTime.Now.AddDays(-7);
                var query = (from j in db.Cidlas
                             where (j.datum > dny) && j.mistnost == "Koupelna"
                             select j.sviceni).Average();
                sviceniLabelStatistika3.Content = query;
            }
        }


        private void LightingLabelStatisticsKitchen(object sender, RoutedEventArgs e)
        {
            using (var db = new ISDatabaseEntities())
            {
                var dny = DateTime.Now.AddDays(-7);
                var query = (from j in db.Cidlas
                             where (j.datum > dny) && j.mistnost == "Kuchyň"
                             select j.sviceni).Average();
                sviceniLabelStatistika4.Content = query;
            }
        }
        private void Statistics_LoadHeating(object sender, RoutedEventArgs e)
        {
            Statistics_UpdateHeating();
        }
        private void Statistics_UpdateHeating()
        {
            using (var db = new ISDatabaseEntities())
            {
                var query = (from j in db.Topenis
                             where j.spusteni == "zapnuto"
                             select j).Count();

                StatistikaTopeni.Content = query;
            }
        }
    }
}
