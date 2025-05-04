using Lab4.Classes;
using Lab4.Classes.ValidationAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    [JsonObject(IsReference = true)]
    public class Executor : IDataErrorInfo, INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;

        public Executor(string firstName, string lastName, DateTime birthDate)
        {
            _firstName = firstName;
            _lastName = lastName;
            _birthDate = birthDate;
        }
        [Required(ErrorMessage = "Ім'я обов'язкове")]
        [NameValidation(ErrorMessage = "Ім'я може містити лише літери, пробіли та дефіси.")]
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        [Required(ErrorMessage = "Прізвище обов'язкове")]
        [NameValidation(ErrorMessage = "Прізвище може містити лише літери, пробіли та дефіси.")]
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        [Required(ErrorMessage = "Дата народження обов'язкова")]
        [MinimumAge(18, ErrorMessage = "Вік повинен бути не менше 18 років.")]
        public DateTime BirthDate
        {
            get => _birthDate;
            set
            {
                _birthDate = value;
                OnPropertyChanged(nameof(BirthDate));
            }
        }
        public string FullName => $"{_firstName} {_lastName}";
        public override string ToString()
        {
            return $"{_firstName} {_lastName} ({_birthDate.ToShortDateString()})";
        }
        public ExecutorDTO ToDTO()
        {
            return new ExecutorDTO(_firstName, _lastName, _birthDate);
        }
        public static Executor FromDTO(ExecutorDTO dto)
        {
            return new Executor(dto.FirstName, dto.LastName, dto.BirthDate);
        }
        private void Update(string firstName, string lastName, DateTime birthday)
        {
            _firstName = firstName;
            _lastName = lastName;
            _birthDate = birthday;
        }
        public string this[string columnName]
        {
            get
            {
                var prop = GetType().GetProperty(columnName);
                if (prop == null) return null!;
                var value = prop.GetValue(this);
                var context = new ValidationContext(this) { MemberName = columnName };
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                bool isValid = Validator.TryValidateProperty(value, context, results);
                return isValid ? string.Empty : results.First().ErrorMessage!;
            }
        }

        public string Error
        {
            get
            {
                var context = new ValidationContext(this);
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                Validator.TryValidateObject(this, context, results, true);
                return string.Join("\n", results.Select(r => r.ErrorMessage));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
    [JsonObject(IsReference = true)]
    public class ExecutorDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public ExecutorDTO(string firstName, string lastName, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }
    }
}
