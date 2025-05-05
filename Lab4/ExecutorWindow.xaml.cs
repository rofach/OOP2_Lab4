using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        bool _cancelled = false;
        public AddExecutorForm(Executor? executor = null)
        {
            InitializeComponent();
            DataContext = _executor = executor ?? new(string.Empty, string.Empty, DateTime.Today.AddYears(-18));
        }
        public Executor ExecutorResult => _executor;

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(CheckValidation())
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Введені дані не валідні", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancelled = true;
            DialogResult = false;
        }
        private bool CheckValidation()
        {
            return Validation.GetHasError(txtFirstName) == false &&
                   Validation.GetHasError(txtLastName) == false &&
                   Validation.GetHasError(dpBirthDate) == false;
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(_cancelled) e.Cancel = false;
            else if (DialogResult != true)
            {
                var res = MessageBox.Show("Зберегти?", "Підтвердження", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    if(CheckValidation())
                    {
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("Введені дані не валідні", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                        e.Cancel = true;
                    }
                    e.Cancel = false;
                }
                else if (res == MessageBoxResult.No) e.Cancel = false;
                else if (res == MessageBoxResult.Cancel) e.Cancel = true;

            }
        }

       
    }
    
}
