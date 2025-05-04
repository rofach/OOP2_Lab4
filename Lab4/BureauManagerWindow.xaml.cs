using System.CodeDom;
using System.CodeDom.Compiler;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for BureauManagerWindow.xaml
    /// </summary>
    public partial class BureauManagerWindow : Window
    {
        private bool _cancelled = false;
        private ServiceBureau _editingBureau;
        private ServiceBureau _originalBureau;
        List<int> _changedIndexes = new List<int>();
        List<Executor> _changedExecutors = new List<Executor>();
        public BureauManagerWindow(ServiceBureau bureau)
        {
            InitializeComponent();
            _originalBureau = bureau;
            _editingBureau = bureau.Clone();
            lbOrders.ItemsSource = _editingBureau.Orders;
            cbExecutors.ItemsSource = _editingBureau.Executors;
            _changedExecutors = _editingBureau.Executors.Select(e => e.Clone()).ToList();
        }

        public ServiceBureau BureauResult => _editingBureau;


        private void btnEditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (lbOrders.SelectedItem is Order selectedOrder)
            {
                var orderWindow = new OrderWindow(selectedOrder, _editingBureau.Executors);
                if (orderWindow.ShowDialog() == true)
                {
                    var updated = orderWindow.OrderResult;
                    if (updated != null)
                    {
                        int idx = _editingBureau.Orders.IndexOf(selectedOrder);
                        _editingBureau.Orders.RemoveAt(idx);
                        _editingBureau.Orders.Insert(idx, updated);
                        lbOrders.Items.Refresh();
                    }
                }
            }
        }

        private void btnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderWindow = new OrderWindow(executors: _editingBureau.Executors);
            if (orderWindow.ShowDialog() == true)
            {
                var order = orderWindow.OrderResult;
                
                if (order != null)
                {
                    _editingBureau.AddOrder(order);
                    _editingBureau.Executors = orderWindow.ExecutorsList;
                    lbOrders.Items.Refresh();
                }
            }
           
        }
        private void btnDetailedInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((Order)lbOrders.SelectedItem).ToString(), "Детальна інформація", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _originalBureau.Orders = _editingBureau.Orders.Select(o => (Order)o.Clone()).ToList();
            foreach(var order in _originalBureau.Orders)
            {
                if(!_originalBureau.Executors.Contains(order.Executor))
                    _originalBureau.Executors.Add(order.Executor);
            }
            foreach(var executor in _editingBureau.Executors)
            {
                if (!_originalBureau.Executors.Contains(executor))
                    _originalBureau.Executors.Add(executor);
            }
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            foreach(var idx in _changedIndexes)
            {
                _editingBureau.Executors[idx].FirstName = _changedExecutors[idx].FirstName;
                _editingBureau.Executors[idx].LastName = _changedExecutors[idx].LastName;
                _editingBureau.Executors[idx].BirthDate = _changedExecutors[idx].BirthDate;
            }
            _cancelled = true;
            DialogResult = false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_cancelled) e.Cancel = false;
            else if (DialogResult != true)
            {
                var res = MessageBox.Show("Зберегти зміни?", "Підтвердження", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    DialogResult = true;
                    e.Cancel = false;
                }
                if (res == MessageBoxResult.Cancel) e.Cancel = true;
            }
           
        }

        private void btnDeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (lbOrders.SelectedItem is Order selectedOrder)
            {
                _editingBureau.Orders.Remove(selectedOrder);
                
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
                    _editingBureau.Executors.Add(executor);
                    cbExecutors.Items.Refresh();
                }
            }
            _changedExecutors = _editingBureau.Executors.Select(e => e.Clone()).ToList();
        }
        private void btnEditExecutor_Click(object sender, RoutedEventArgs e)
        {
            if(cbExecutors.SelectedItem is Executor selectedExecutor)
            {
                int idx = _editingBureau.Executors.IndexOf(selectedExecutor);
                var window = new AddExecutorForm(selectedExecutor);
                if (window.ShowDialog() == true)
                {
                    var copy = selectedExecutor;
                   
                    var updated = window.ExecutorResult;
                    if (updated != null)
                    {
                        
                        _editingBureau.Executors.RemoveAt(idx);
                        _editingBureau.Executors.Insert(idx, updated);
                        foreach (var order in _editingBureau.Orders)
                        {
                            if (order.Executor == selectedExecutor)
                            {
                                order.Executor = updated;
                            }
                        }
                        _changedIndexes.Add(idx);
                        selectedExecutor = updated;

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
                int idx = _editingBureau.Executors.IndexOf(executor);
                _changedIndexes.Remove(idx);
                var res = MessageBox.Show("Ви точно хочете видалити?", "Підтвердження", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if(res == MessageBoxResult.No || res == MessageBoxResult.Cancel) return;
                _editingBureau.Executors.Remove(executor);
                List<Order> ordersToRemove = new List<Order>();
                foreach (var order in _editingBureau.Orders)
                {
                    if (order.Executor == executor)
                    {
                        ordersToRemove.Add(order);
                    }
                }
                _editingBureau.Orders.RemoveAll(o => ordersToRemove.Contains(o));
            }
            cbExecutors.Items.Refresh();
            lbOrders.Items.Refresh();
            btnDeleteExecutor.IsEnabled = false;
            btnEditExecutor.IsEnabled = false;
            _changedExecutors = _editingBureau.Executors.Select(e => e.Clone()).ToList();
        }

        private void cbExecutors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnEditExecutor.IsEnabled = true;
            btnDeleteExecutor.IsEnabled = true;
        }

        
    }
}
