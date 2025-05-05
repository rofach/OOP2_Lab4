using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
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
        private const string FilePath = "bureaus.json";
        private const string LogFilePath = "validation_log.txt";
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            lbBureaus.ItemsSource = _bureaus;
        }

        private void LoadData()
        {
            if (File.Exists(LogFilePath))
                File.Delete(LogFilePath);
            
            List<object> problemObjects = new();
            var settings = new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                MissingMemberHandling = MissingMemberHandling.Error,
                Error = (sender, args) =>
                {
                    AppendLogText($"[Parse Error] Path: {args.ErrorContext.Path}, Message: {args.ErrorContext.Error.Message}");
                    problemObjects.Add(args.CurrentObject as Order);
                    args.ErrorContext.Handled = true;
                }
            };

            if (!File.Exists(FilePath))
            {
                _bureaus = new List<ServiceBureau>();
                return;
            }

            string json = File.ReadAllText(FilePath);
            var readedList = JsonConvert.DeserializeObject<List<ServiceBureau>>(json, settings) ?? new List<ServiceBureau>();
            var validList = new List<ServiceBureau>();
            foreach(var bureau in readedList)
            {
                List<Executor> problemExecutors = new();
                List<Order> validOrders = new();
                foreach (var executor in bureau.Executors)
                {
                    var exError = Validate(executor);
                    if (exError.Count > 0)
                    {
                        AppendLogText($"[Validation Error] Errors: {executor} {string.Join(", ", exError)}");
                        problemExecutors.Add(executor);
                    }
                }
                readedList.Where(b => b.Executors.Contains(problemExecutors.FirstOrDefault())).ToList().ForEach(b => b.Executors.Remove(problemExecutors.FirstOrDefault()));
                foreach (var order in bureau.Orders)
                {
                    var exError = Validate(order);
                    if (exError.Count > 0)
                    {
                        AppendLogText($"[Validation Error] Errors: {order} ------ {string.Join(", ", exError)}");
                    }
                    else if(!problemObjects.Contains(order))
                        validOrders.Add(order);
                }
                bureau.Orders = validOrders;
                var bureauErrors = Validate(bureau);
                if(bureauErrors.Count > 0)
                {
                    AppendLogText($"[Validation Error] Errors: {bureau} {string.Join(", ", bureauErrors)}");
                    continue;
                }
                validList.Add(bureau);
            }

            _bureaus = validList;
        }
        private List<string> Validate(object obj)
        {
            var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            var context = new ValidationContext(obj);
            Validator.TryValidateObject(obj, context, results, true);
            foreach(var prop in obj.GetType().GetProperties())
            {
                if(prop.PropertyType == typeof(string) || prop.PropertyType.IsValueType)
                    continue;
                var value = prop.GetValue(obj);
               
                if(value is IEnumerable<object> enumerable)
                {
                    foreach (var item in enumerable)
                    {
                        var itemResults = Validate(item);
                        foreach (var res in itemResults)
                        {
                            results.Add(new System.ComponentModel.DataAnnotations.ValidationResult(res));
                        }
                    }
                }
                else if (value != null)
                {
                    var itemResults = Validate(value);
                    foreach(var res in itemResults)
                        results.Add(new System.ComponentModel.DataAnnotations.ValidationResult(res));
                }
            }

            return results.Select(r => r.ErrorMessage).ToList();
        }
        private void AppendLogText(string message)
        {
            File.AppendAllText(LogFilePath, message + '\n');
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
                    //selectedBureau = updated;
                    //lbBureaus.Items.Refresh();
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