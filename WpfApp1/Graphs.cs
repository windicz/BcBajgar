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
        private List<object> CollectionTotalConsumption;
        private List<object> CollectionAverage;
        private void Graph_Lighting(object sender, RoutedEventArgs e)
        {
            using (var db = new ISDatabaseEntities())
            {
                CollectionAverage = new List<object>();

                var query = from u in db.Cidlas
                            where (u.mistnost == comboboxGraf.Text.ToString())
                            group u by u.datum.Month
                            into g
                            select new { Mesic = g.Key, Prumerne_sviceni = g.Average(v => v.sviceni) };

                foreach (var item in query)
                {
                    CollectionAverage.Add(item);
                }

                ((ColumnSeries)MyChart3.Series[0]).ItemsSource = CollectionAverage;

            }
        }
        private void Graph_PowerConsumption(object sender, RoutedEventArgs e)
        {
            using (var db = new ISDatabaseEntities())
            {

                CollectionTotalConsumption = new List<object>();
                var query = from u in db.Cidlas
                            group u by u.datum.Month
                            into g
                            select new { Mesic = g.Key, Spotreba = g.Sum(v => v.spotreba) };

                foreach (var item in query)
                {
                    CollectionTotalConsumption.Add(item);
                }
               ((ColumnSeries)MyChart2.Series[0]).ItemsSource = CollectionTotalConsumption;

            }
        }

    }
}
