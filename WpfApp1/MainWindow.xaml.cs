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


    }
}