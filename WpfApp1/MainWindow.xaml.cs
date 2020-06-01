using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls.DataVisualization.Charting;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<object> CelkemSpotreba;
        private HubConnection _connection;
        private List<object> CollectionPrumer;

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

        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {

            _connection.On<string>("Connected",
                                   (connectionid) =>
            {

                tbMain.Text = connectionid;
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


        private void Button_Click(object sender, RoutedEventArgs e)
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


        }

        public void ZvolitTeplotu(object sender, RoutedEventArgs e)
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
                        ZapnutiTopeniVDatabazi();
                }
                if (teplota.Convertor(tretiTeplota.Text) >= teplota.Convertor(TeplotaKoupelnaZadana.Text))
                {
                    TopeniJeKoupelna.Content = "vypnuté.";
                }
                else
                {
                    TopeniJeKoupelna.Content = "zapnuté.";
                        ZapnutiTopeniVDatabazi();
                }
                if (teplota.Convertor(druhaTeplota.Text) >= teplota.Convertor(TeplotaLozniceZadana.Text))
                {
                    TopeniJeLoznice.Content = "vypnuté.";
                }
                else
                {
                    TopeniJeLoznice.Content = "zapnuté.";
                        ZapnutiTopeniVDatabazi();
                }
                if (teplota.Convertor(ctvrtaTeplota.Text) >= teplota.Convertor(TeplotaKuchynZadana.Text))
                {
                    TopeniJeKuchyn.Content = "vypnuté.";
                }
                else
                {
                    TopeniJeKuchyn.Content = "zapnuté.";
                        ZapnutiTopeniVDatabazi();
                }
            }
            catch (Exception) { 
                throw new Exception(); 
            }

            UpdateStatistikaTopeni();
            
        }
        public void ZapnutiTopeniVDatabazi()
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

        private void StatistikaTopeni_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateStatistikaTopeni();
        }
        private void UpdateStatistikaTopeni()
        {
            using (var db = new ISDatabaseEntities())
            {
                var query = (from j in db.Topenis
                             where j.spusteni == "zapnuto"
                             select j).Count();

                StatistikaTopeni.Content = query;
            }
        }

        private void MyChart2_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new ISDatabaseEntities())
            {

                CelkemSpotreba = new List<object>();
                var query = from u in db.Cidlas
                            group u by u.datum.Month
                            into g
                            select new { Mesic = g.Key, Spotreba = g.Sum(v => v.spotreba) };

                foreach (var item in query)
                {
                    CelkemSpotreba.Add(item);
                }
               ((ColumnSeries)MyChart2.Series[0]).ItemsSource = CelkemSpotreba;

            }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            comboboxGraf.Items.Add("Obývací pokoj");
            comboboxGraf.Items.Add("Ložnice");
            comboboxGraf.Items.Add("Koupelna");
            comboboxGraf.Items.Add("Kuchyň");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var db = new ISDatabaseEntities())
            {
                CollectionPrumer = new List<object>();

                var query = from u in db.Cidlas
                            where (u.mistnost == comboboxGraf.Text.ToString())
                            group u by u.datum.Month
                            into g
                            select new { Mesic = g.Key, Prumerne_sviceni = g.Average(v => v.sviceni) };

                foreach (var item in query)
                {
                    CollectionPrumer.Add(item);
                }

                ((ColumnSeries)MyChart3.Series[0]).ItemsSource = CollectionPrumer;

            }
        }

        private void sviceniLabelStatistika_Loaded(object sender, RoutedEventArgs e)
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

        private void detailObyvakLabel_Loaded(object sender, RoutedEventArgs e)
        {
            detailObyvakLabel.Content = TopeniJeObyvak.Content;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
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
                ZapnutiTopeniVDatabazi();
            }
        }

        private void DetailTeplotaObyvak_Loaded(object sender, RoutedEventArgs e)
        {
            DetailTeplotaObyvak.Text = prvniTeplota.Text;
        }

        private void DetailSpotrebaObyvak_Loaded(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaObyvak.Text = prvniSpotreba.Text;
        }

        private void DetailTeplotaLoznice_Loaded(object sender, RoutedEventArgs e)
        {
            DetailTeplotaLoznice.Text = druhaTeplota.Text;
        }

        private void DetailSpotrebaLoznice_Loaded(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaLoznice.Text = druhaSpotreba.Text;
        }

        private void sviceniLabelStatistika2_Loaded(object sender, RoutedEventArgs e)
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

        private void detailLozniceLabel_Loaded(object sender, RoutedEventArgs e)
        {
            detailLozniceLabel.Content = TopeniJeLoznice.Content;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
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
                ZapnutiTopeniVDatabazi();
            }
        }

        private void DetailTeplotaKoupelna_Loaded(object sender, RoutedEventArgs e)
        {
            DetailTeplotaKoupelna.Text = tretiTeplota.Text;
        }

        private void DetailSpotrebaKoupelna_Loaded(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaKoupelna.Text = tretiSpotreba.Text;
        }

        private void sviceniLabelStatistika3_Loaded(object sender, RoutedEventArgs e)
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

        private void detailKoupelnaLabel_Loaded(object sender, RoutedEventArgs e)
        {
            detailKoupelnaLabel.Content = TopeniJeKoupelna.Content;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
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
                ZapnutiTopeniVDatabazi();
            }
        }

        private void DetailTeplotaKuchyn_Loaded(object sender, RoutedEventArgs e)
        {
            DetailTeplotaKuchyn.Text = ctvrtaTeplota.Text;
        }

        private void DetailSpotrebaKuchyn_Loaded(object sender, RoutedEventArgs e)
        {
            DetailSpotrebaKuchyn.Text = ctvrtaSpotreba.Text;
        }

        private void sviceniLabelStatistika4_Loaded(object sender, RoutedEventArgs e)
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

        private void detailKuchynLabel_Loaded(object sender, RoutedEventArgs e)
        {
            detailKuchynLabel.Content = TopeniJeKuchyn.Content;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
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
                ZapnutiTopeniVDatabazi();
            }
        }

    }
}