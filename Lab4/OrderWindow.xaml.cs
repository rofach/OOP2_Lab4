using Microsoft.Windows.Themes;
using System;
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

namespace Lab4
{
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        private Order? _order;

        public OrderWindow(OrderDTO? order = null)
        {
            InitializeComponent();
            cbService.ItemsSource = Enum.GetValues(typeof(ServiceType));
            if (order != null)
            {
                _order = Order.FromDTO(order);
                dpDate.SelectedDate = _order.OrderDate;
                
                cbService.SelectedItem = _order.Customer.Service;
                txtCost.Text = _order.Cost.ToString();
            }
        }     
        
        public OrderDTO? OrderResult => _order == null ? null : _order.ToDTO();        
        private bool Validate()
        {
            return !(String.IsNullOrEmpty(txtAddress.Text) ||
                   String.IsNullOrEmpty(txtCost.Text) ||
                   cbService.SelectedItem == null);
        }
        
        private void SaveData()
        {
            int num = int.Parse(txtCost.Text);
            _order = new Order(
                   new Executor("Виконавець", "Виконавець", (DateTime)dpDate.SelectedDate),
                   new Customer((ServiceType)cbService.SelectedItem, txtAddress.Text),
                   dpDate.SelectedDate.Value,
                   num
               );
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //_order = new Order()
            if (Validate())
            {
                SaveData();
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Заповніть всі поля!", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult != true)
            {
                var res = MessageBox.Show("Зберегти зміни?", "Підтвердження", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    SaveData();
                    e.Cancel = false;
                }
                if (res == MessageBoxResult.Cancel) e.Cancel = true;
            }
        }

        private void cbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}