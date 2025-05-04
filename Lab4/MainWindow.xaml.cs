using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

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
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };
            if(!File.Exists(FilePath))
            {
                _bureaus = new();
                return;
            }
            string json = File.ReadAllText(FilePath);
            _bureaus = JsonConvert.DeserializeObject<List<ServiceBureau>>(json, settings) ?? new List<ServiceBureau>();
        }

        private void SaveData()
        {
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            };
            string json = JsonConvert.SerializeObject(_bureaus, settings);
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
                var window = new BureauManagerWindow(selectedBureau);
                if (window.ShowDialog() == true)
                {
                    var updated = window.BureauResult;
                    selectedBureau = updated;
                    lbBureaus.Items.Refresh();
                    /*int idx = _bureaus.IndexOf(selectedBureau);
                    _bureaus.RemoveAt(idx);
                    _bureaus.Insert(idx, updated);
                    lbBureaus.Items.Refresh();*/
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

            btnEditBureau.IsEnabled = true;
            btnDeleteBureau.IsEnabled = true;
            btnDetailedInfo.IsEnabled = true;
        }

        private void btnDeleteBureau_Click(object sender, RoutedEventArgs e)
        {
            if(lbBureaus.SelectedItem is ServiceBureau selectedBureau)
            {
                if (MessageBox.Show($"Ви впевнені, що хочете видалити бюро \"{selectedBureau.BureauName}\"?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _bureaus.Remove(selectedBureau);
                    lbBureaus.Items.Refresh();
                }
            }
        }
    }
}