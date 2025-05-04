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
using System.ComponentModel.DataAnnotations;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private Order _order;
        private List<Executor>? _executors;
        private bool _cancelled = false;
        public OrderWindow(Order? order = null, List<Executor>? executors = null) 
        {
            InitializeComponent();
            _order = order ?? new Order() { Customer = new() };
            this.DataContext = _order;
            if (executors != null)
                _executors = executors;
            else
                _executors = new();
           
            cbExecutors.ItemsSource = executors;
            var services = Enum.GetValues(typeof(ServiceType)).Cast<ServiceType>().Select(s => new { Value = s, Description = EnumDescription.GetDescription(s) }).ToList();
            cbService.ItemsSource = services;
            cbService.DisplayMemberPath = "Description";
            cbService.SelectedValuePath = "Value";
            if(order != null)
            {
                cbExecutors.SelectedItem = order.Executor;
                cbService.SelectedValue = order.Customer.Service;
            }
        }

        public Order? OrderResult => _order ?? null;
        public List<Executor>? ExecutorsList => _executors ?? null; 
      
        private void SaveData()
        {
            _order.OrderDate = dpDate.SelectedDate ?? DateTime.Today;
            _order.Executor = cbExecutors.SelectedItem as Executor;
            _order.Customer = new Customer((ServiceType)cbService.SelectedValue, txtAddress.Text);
            DialogResult = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbExecutors.SelectedItem == null)
            {
                MessageBox.Show("Виберіть виконавця.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (cbService.SelectedItem == null)
            {
                MessageBox.Show("Виберіть послугу.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (Validation.GetHasError(txtCost) ||
                Validation.GetHasError(txtAddress) ||
                Validation.GetHasError(dpDate))
            {
                MessageBox.Show("Виправте помилки у формі.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveData();
            Close();
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
                    e.Cancel = false;                                       
                }
                if (res == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }

        private void btnAddExecutor_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddExecutorForm(_executors);
            if(window.ShowDialog() == true)
            {
                var executor = window.ExecutorResult;
                if (executor != null)
                {
                    _executors.Add(Executor.FromDTO(executor));
                    cbExecutors.Items.Refresh();
                }
            }
           
        }
    }
}