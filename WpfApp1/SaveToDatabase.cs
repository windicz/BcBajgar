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
        public void Database_TemperatureOn()
        {
            try
            {
                using (var context = new ISDatabaseEntities())
                {

                    var query2 = context.Topenis.SqlQuery("SELECT TOP 1 * FROM Topeni ORDER BY Id DESC").ToList();
                    var query3 = query2.Select(u => u.Id).ToList().First();
                    var temp = Convert.ToInt32(query3);
                    var result23 = context.Topenis.SingleOrDefault(b => b.Id == temp);
                    result23.spusteni = "zapnuto";
                    context.SaveChanges();

                }
            }
            catch (Exception)
            {

                throw new Exception();
            }

        }
        private void Button_Click_Sensors(object sender, RoutedEventArgs e)
        {
            try
            {
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
                using (var db = new ISDatabaseEntities())
                {

                    var query2 = db.Topenis.SqlQuery("SELECT TOP 1 * FROM Topeni ORDER BY Id DESC").ToList();
                    var query = query2.Select(u => u.Id).ToList().First();
                    Tmp.Text = Convert.ToString(query);

                    using (ISDatabaseEntities context = new ISDatabaseEntities())
                    {
                        Cidla cus = new Cidla()
                        {
                            mistnost = obyvaciPokojLabel.Content.ToString(),
                            teplota = Int32.Parse(prvniTeplota.Text),
                            spotreba = Int32.Parse(prvniSpotreba.Text),
                            sviceni = Int32.Parse(prvniSviceni.Text),
                            datum = DateTime.Now,
                            TopeniId = Int32.Parse(Tmp.Text)
                        };

                        context.Cidlas.Add(cus);
                        context.SaveChanges();

                    }
                    using (ISDatabaseEntities context = new ISDatabaseEntities())
                    {
                        Cidla cus = new Cidla()
                        {
                            mistnost = LozniceLabel.Content.ToString(),
                            teplota = Int32.Parse(druhaTeplota.Text),
                            spotreba = Int32.Parse(druhaSpotreba.Text),
                            sviceni = Int32.Parse(druhaSviceni.Text),
                            datum = DateTime.Now,
                            TopeniId = Int32.Parse(Tmp.Text)
                        };
                        context.Cidlas.Add(cus);
                        context.SaveChanges();
                    }
                    using (ISDatabaseEntities context = new ISDatabaseEntities())
                    {
                        Cidla cus = new Cidla()
                        {
                            mistnost = KoupelnaLabel.Content.ToString(),
                            teplota = Int32.Parse(tretiTeplota.Text),
                            spotreba = Int32.Parse(tretiSpotreba.Text),
                            sviceni = Int32.Parse(tretiSviceni.Text),
                            datum = DateTime.Now,
                            TopeniId = Int32.Parse(Tmp.Text)
                        };
                        context.Cidlas.Add(cus);
                        context.SaveChanges();
                    }
                    using (ISDatabaseEntities context = new ISDatabaseEntities())
                    {
                        Cidla cus = new Cidla()
                        {
                            mistnost = KuchynLabel.Content.ToString(),
                            teplota = Int32.Parse(ctvrtaTeplota.Text),
                            spotreba = Int32.Parse(ctvrtaSpotreba.Text),
                            sviceni = Int32.Parse(ctvrtaSviceni.Text),
                            datum = DateTime.Now,
                            TopeniId = Int32.Parse(Tmp.Text)
                        };
                        context.Cidlas.Add(cus);
                        context.SaveChanges();
                    }
                }
                MessageBox.Show("Údaje z čidel byly úspěšně uloženy.");
            }
            catch (Exception)
            {

                MessageBox.Show("musí být zadány jen číselné hodnoty");
            }

        }
    }
}
