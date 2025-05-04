using System.CodeDom;
using System.CodeDom.Compiler;
using System.Windows;
using System.Windows.Controls;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for BureauManagerWindow.xaml
    /// </summary>
    public partial class BureauManagerWindow : Window
    {
        private ServiceBureau _bureau;
        private List<Executor> _executors;
        private bool _cancelled = false;
        public BureauManagerWindow(ServiceBureau bureau)
        {
            InitializeComponent();
            _bureau = bureau;
            _executors = _bureau.Executors;
            lbOrders.ItemsSource = _bureau.Orders;
            cbExecutors.ItemsSource = _executors;
        }

        public ServiceBureau BureauResult => _bureau;


        private void btnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (lbOrders.SelectedItem is Order selectedOrder)
            {
                var orderWindow = new OrderWindow(selectedOrder, _bureau.Executors);
                if (orderWindow.ShowDialog() == true)
                {
                    var updated = orderWindow.OrderResult;
                    if (updated != null)
                    {
                        int idx = _bureau.Orders.IndexOf(selectedOrder);
                        _bureau.Orders.RemoveAt(idx);
                        _bureau.Orders.Insert(idx, updated);
                        lbOrders.Items.Refresh();
                    }
                }
            }
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow(executors: _bureau.Executors);
            if (orderWindow.ShowDialog() == true)
            {
                var order = orderWindow.OrderResult;
                
                if (order != null)
                {
                    _bureau.AddOrder(order); 
                    _bureau.Executors = orderWindow.ExecutorsList;
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
            MessageBox.Show(((Order)lbOrders.SelectedItem).ToString(), "Детальна інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            
            SaveData();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancelled = true;
            DialogResult = false;
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_cancelled) e.Cancel = false;
            else if (DialogResult != true)
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

        private void lbOrders_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            btnEditOrder.IsEnabled = true;
            btnDeleteOrder.IsEnabled = true;
            btnDetailedInfo.IsEnabled = true;
        }

        private void btnAddExecutor_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddExecutorForm();
            if (window.ShowDialog() == true)
            {
                var executor = window.ExecutorResult;
                if (executor != null)
                {
                    _executors.Add(Executor.FromDTO(executor));
                    cbExecutors.Items.Refresh();
                }
            }
        }
        private void btnEditExecutor_Click(object sender, RoutedEventArgs e)
        {  
            if(cbExecutors.SelectedItem is Executor selectedExecutor)
            {
                var window = new AddExecutorForm(selectedExecutor);
                if (window.ShowDialog() == true)
                {
                    var updated = window.ExecutorResult;
                    if (updated != null)
                    {
                        int idx = _executors.IndexOf(selectedExecutor);
                        _executors.RemoveAt(idx);
                        _executors.Insert(idx, Executor.FromDTO(updated));
                        foreach (var order in _bureau.Orders)
                        {
                            if (order.Executor == selectedExecutor)
                            {
                                order.Executor = _executors[idx];
                            }
                        }
                        lbOrders.Items.Refresh();
                        cbExecutors.Items.Refresh();
                        btnDeleteExecutor.IsEnabled = false;
                        btnEditExecutor.IsEnabled = false;
                    }
                }
                
            }

        }
        private void btnDeleteExecutor_Click(object sender, RoutedEventArgs e)
        {
            if (cbExecutors.SelectedItem is Executor executor)
            {
                var res = MessageBox.Show("Ви точно хочете видалити?", "Підтвердження", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if(res == MessageBoxResult.No || res == MessageBoxResult.Cancel) return;
                _executors.Remove(executor);
                List<Order> ordersToRemove = new List<Order>();
                foreach (var order in _bureau.Orders)
                {
                    if (order.Executor == executor)
                    {
                        ordersToRemove.Add(order);
                    }
                }
                _bureau.Orders.RemoveAll(o => ordersToRemove.Contains(o));
            }
            cbExecutors.Items.Refresh();
            lbOrders.Items.Refresh();
            btnDeleteExecutor.IsEnabled = false;
            btnEditExecutor.IsEnabled = false;
        }
      

        private void cbExecutors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditExecutor.IsEnabled = true;
            btnDeleteExecutor.IsEnabled = true;
        }

        
    }
}
