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
        private static readonly Regex NameRegex = new Regex("^[A-Za-zА-Яа-я]+$", RegexOptions.Compiled);
        private const int MinNameLength = 2;
        private const int MaxNameLength = 30;
        public AddExecutorForm(List<Executor> executors)
        {
            InitializeComponent();
        }
        public ExecutorDTO? ExecutorResult => _executor.ToDTO();

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                _executor = new Executor(txtFirstName.Text, txtLastName.Text, dpBirthDate.SelectedDate.Value);
                DialogResult = true;
            }
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

        private bool Validate()
        {
            var firstName = txtFirstName.Text.Trim();
            if (string.IsNullOrEmpty(firstName) || firstName.Length < MinNameLength || firstName.Length > MaxNameLength)
            {
                MessageBox.Show(
                    $"Ім'я повинно містити від {MinNameLength} до {MaxNameLength} букв.",
                    "Помилка введення",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                txtFirstName.Focus();
                return false;
            }
            if (!NameRegex.IsMatch(firstName))
            {
                MessageBox.Show(
                    "Недопустимі символи в імені",
                    "Помилка введення",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                txtFirstName.Focus();
                return false;
            }
            
            var lastName = txtLastName.Text.Trim();
            if (string.IsNullOrEmpty(lastName) || lastName.Length < MinNameLength || lastName.Length > MaxNameLength)
            {
                MessageBox.Show(
                    $"Ghspdbot повинно містити від {MinNameLength} до {MaxNameLength} букв.",
                    "Помилка введення",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                txtLastName.Focus();
                return false;
            }
            if (!NameRegex.IsMatch(lastName))
            {
                MessageBox.Show(
                    "Недопустимі символи в імені",
                    "Помилка введення",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                txtLastName.Focus();
                return false;
            }

            if (!dpBirthDate.SelectedDate.HasValue)
            {
                MessageBox.Show(
                    "Вкажіть дату народження",
                   "Помилка введення",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                dpBirthDate.Focus();
                return false;
            }

            var birthday = dpBirthDate.SelectedDate.Value;
            var today = DateTime.Today;
            var adultDate = today.AddYears(-18);
            if (birthday > adultDate)
            {
                MessageBox.Show(
                    "Виконавець повинен бути повнолітнім",
                    "Помилка введення",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                dpBirthDate.Focus();
                return false;
            }

            return true;
        }
    }
    
}
