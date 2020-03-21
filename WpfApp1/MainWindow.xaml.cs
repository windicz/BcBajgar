using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HubConnection _connection;

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
    }
}