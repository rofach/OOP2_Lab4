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
    /// Interaction logic for AddExecutorForm.xaml
    /// </summary>
    public partial class AddExecutorForm : Window
    {
        private Executor _executor;
        public AddExecutorForm(List<Executor> executors)
        {
            InitializeComponent();
        }
        public ExecutorDTO? ExecutorResult => _executor.ToDTO();

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _executor = new Executor(txtFirstName.Text, txtLastName.Text, dpBirthDate.SelectedDate.Value);

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
