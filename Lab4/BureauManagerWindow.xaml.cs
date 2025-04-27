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
    /// Interaction logic for BureauManagerWindow.xaml
    /// </summary>
    public partial class BureauManagerWindow : Window
    {
        private ServiceBureau _bureau;
        public BureauManagerWindow(ServiceBureauDTO bureau)
        {
            InitializeComponent();
            _bureau = ServiceBureau.FromDTO(bureau);
            lbOrders.ItemsSource = _bureau.Orders;
        }

        public ServiceBureauDTO BureauResult => _bureau.ToDTO();
       

        private void btnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow();
            if (lbOrders.SelectedItem is Order selectedOrder)
            {
                orderWindow = new OrderWindow(selectedOrder.ToDTO());
                if (orderWindow.ShowDialog() == true)
                {
                    var order = orderWindow.OrderResult;
                    if (order != null)
                    {
                        selectedOrder = Order.FromDTO(order);
                        lbOrders.Items.Refresh();
                    }
                }
            }
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow();
            if(orderWindow.ShowDialog() == true)
            {
                var order = orderWindow.OrderResult;
                if (order != null)
                {
                    _bureau.AddOrder(Order.FromDTO(order));
                    //lbOrders.Items.Add(Order.FromDTO(order));
                    lbOrders.Items.Refresh();
                }
            }
            
        }
        private void SaveData()
        {
            DialogResult = true;
        }
        private void btnDetailedInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveData();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
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

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (lbOrders.SelectedItem is Order selectedOrder)
            {
                _bureau.Orders.Remove(selectedOrder);
                lbOrders.Items.Refresh();
            }
        }
    }
}
