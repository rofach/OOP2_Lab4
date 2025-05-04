using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddExecutorForm.xaml
    /// </summary>
    public partial class AddExecutorForm : Window
    {
        private Executor _executor;
        private const int MinNameLength = 2;
        private const int MaxNameLength = 30;

        public AddExecutorForm(Executor? executor = null)
        {
            InitializeComponent();
            DataContext = _executor = executor ?? new(string.Empty, string.Empty, DateTime.Today.AddYears(-18));
        }
        public ExecutorDTO? ExecutorResult => _executor.ToDTO();

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;

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
                if (MessageBox.Show("Ви впевнені, що хочете закрити вікно?", "Закриття вікна", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

       
    }
    
}
