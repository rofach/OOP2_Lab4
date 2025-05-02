using Microsoft.Windows.Themes;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Lab4;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private Order? _order;
        private List<Executor>? _executors;
        private bool _cancelled = false;

        public OrderWindow(Order? order = null, List<Executor>? executors = null) // (OrderDTO? order = null, List<ExecutorDTO>? executors = null)
        {
            InitializeComponent();
            //cbService.ItemsSource = Enum.GetValues(typeof(ServiceType));
            
            if (executors != null)
                _executors = executors;
            else
                _executors = new();

            cbExecutors.ItemsSource = executors;
            //var services = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().Select(s => new {Value = s,Description = s.GetDescription()}).ToList();
            var services = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().Select(s => new { Value = s, Description = EnumDescription.GetDescription(s) }).ToList();
            cbService.ItemsSource = services;
            cbService.DisplayMemberPath = "Description";
            cbService.SelectedValuePath = "Value";
            if (order != null)
            {
                _order = order;
                dpDate.SelectedDate = _order.OrderDate;
                cbExecutors.SelectedItem = _executors[_executors.IndexOf(_order.Executor)];
                cbService.SelectedItem = services.FirstOrDefault(s => s.Value == _order.Customer.Service);
                cbService.SelectedValue = _order.Customer.Service;
                txtCost.Text = _order.Cost.ToString();
                txtAddress.Text = _order.Customer.Address;
                
            }
            else
            {
                dpDate.SelectedDate = DateTime.Today;
            }
        }

        public Order? OrderResult => _order ?? null; //public OrderDTO? OrderResult => _order == null ? null : _order.ToDTO();
        public List<Executor>? ExecutorsList => _executors ?? null; //public List<ExecutorDTO>? ExecutorsList => _executors?.Select(ex=>ex.ToDTO()).ToList();
        private bool Validate()
        {
            return dpDate.SelectedDate != null &&
                   cbService.SelectedItem != null &&
                   cbExecutors.SelectedItem != null &&
                   !string.IsNullOrWhiteSpace(txtAddress.Text) &&
                   !string.IsNullOrWhiteSpace(txtCost.Text) &&
                   int.TryParse(txtCost.Text, out int cost) && cost > 0;
        }

        private void SaveData()
        {
            int num = int.Parse(txtCost.Text);
            _order = new Order(
                   (Executor)cbExecutors.SelectedItem,
                   new Customer((ServiceType)cbService.SelectedValue, txtAddress.Text),
                   dpDate.SelectedDate.Value.Date,
                   num
               );
            DialogResult = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                SaveData();
                Close();
            }
            else
            {
                MessageBox.Show("Правильно заповніть поля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancelled = true;
            DialogResult = false;
            
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_cancelled) e.Cancel = false;
            else if (DialogResult != true)
            {
                var res = MessageBox.Show("Зберегти зміни?", "Підтвердження", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if (Validate())
                    {
                        SaveData();
                        e.Cancel = false;
                    }
                    else
                        MessageBox.Show("Заповніть всі поля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (res == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }

        private void btnAddExecutor_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddExecutorForm(_executors);//_executors.Select(ex => ex.ToDTO()).ToList());
            if(window.ShowDialog() == true)
            {
                var executor = window.ExecutorResult;
                if (executor != null)
                {
                    _executors.Add(Executor.FromDTO(executor));//(Executor.FromDTO(executor));
                    cbExecutors.Items.Refresh();
                }
            }
           
        }
    }
}