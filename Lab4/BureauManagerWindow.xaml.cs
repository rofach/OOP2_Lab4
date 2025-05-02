using System.Windows;

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
        public BureauManagerWindow(ServiceBureau bureau) //public BureauManagerWindow(ServiceBureauDTO bureau)
        {
            InitializeComponent();
            _bureau = bureau;
            _executors = _bureau.Executors;
            lbOrders.ItemsSource = _bureau.Orders;
        }

        public ServiceBureau BureauResult => _bureau; // public ServiceBureauDTO BureauResult => _bureau.ToDTO();


        private void btnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (lbOrders.SelectedItem is Order selectedOrder)
            {
                var orderWindow = new OrderWindow(selectedOrder, _bureau.Executors); // new OrderWindow(selectedOrder.ToDTO(), _bureau.Executors.Select(ex => ex.ToDTO()).ToList());
                if (orderWindow.ShowDialog() == true)
                {
                    var updated = orderWindow.OrderResult;// ServiceBureau.FromDTO(window.BureauResult);
                    if (updated != null)
                    {

                        int idx = _bureau.Orders.IndexOf(selectedOrder);
                        _bureau.Orders.RemoveAt(idx);
                        _bureau.Orders.Insert(idx, updated);
                        lbOrders.Items.Refresh();
                        btnEditOrder.IsEnabled = false;
                        btnDeleteOrder.IsEnabled = false;
                        btnDetailedInfo.IsEnabled = false;
                    }

                }
            }
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow(executors: _bureau.Executors); //  new OrderWindow(executors: _bureau.Executors.Select(ex => ex.ToDTO()).ToList());
            if (orderWindow.ShowDialog() == true)
            {
                var order = orderWindow.OrderResult;
                
                if (order != null)
                {
                    _bureau.AddOrder(order); //_bureau.AddOrder(Order.FromDTO(order));
                    _bureau.Executors = orderWindow.ExecutorsList; //  _bureau.Executors = orderWindow.ExecutorsList?.Select(ex => Executor.FromDTO(ex)).ToList() ?? new List<Executor>();
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
    }
}
