using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Lab4;
namespace Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<ServiceBureau> _bureaus = new();
        const string FilePath = "bureaus.json";
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            lbBureaus.ItemsSource = _bureaus;
        }

        private void LoadData()
        {
           
        }

        private void SaveData()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var dtoList = _bureaus.Select(c => c.ToDTO()).ToList();
            string json = JsonSerializer.Serialize(dtoList, options);
            File.WriteAllText(FilePath, json);
        }

        private void BtnAddBureau_Click(object sender, RoutedEventArgs e)
        {
            
            string name = String.IsNullOrEmpty(txtBureauName.Text) ? ("Бюро " + (_bureaus.Count + 1)) : txtBureauName.Text;
            var bureau = new ServiceBureau(name);
            _bureaus.Add(bureau);
            lbBureaus.Items.Refresh();
        }

        private void BtnEditBureau_Click(object sender, RoutedEventArgs e)
        {
            if (lbBureaus.SelectedItem is ServiceBureau selectedBureau)
            {
                var window = new BureauManagerWindow(selectedBureau.ToDTO());
                if (window.ShowDialog() == true)
                {
                    var updated = ServiceBureau.FromDTO(window.BureauResult);
                    int idx = _bureaus.IndexOf(selectedBureau);
                    _bureaus.RemoveAt(idx);
                    _bureaus.Insert(idx, updated);
                    lbBureaus.Items.Refresh();
                }
            }
            
        }
        private void btnDetailedInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(lbBureaus.SelectedItem.ToString(), "Детальна інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveData();
        }

        private void lbBureaus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbBureaus.SelectedItem is ServiceBureau selectedBureau)
                btnEditOrder.IsEnabled = true;
            else
                btnEditOrder.IsEnabled = false;
        }
    }
}