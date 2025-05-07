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
    public class Customer : IDataErrorInfo, INotifyPropertyChanged
    {
        private ServiceType _service;
        
        private string _address;

        public Customer()
        {
            _service = ServiceType.Cleaning;
            _address = string.Empty;
        }
        public Customer(ServiceType service, string address)
        {
            _service = service;
            _address = address;
        }
        [Required(ErrorMessage = "Адреса обов'язкова")]
        public ServiceType Service
        {
            get => _service;
            set
            {
                _service = value;
                OnPropertyChanged(nameof(Service));
            }
        }

        [Required(ErrorMessage = "Адреса обов'язкова")]
        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public override string ToString()
        {
            string serviceName = EnumDescription.GetDescription(_service);
            return $"Послуга: {serviceName}, Адреса: {_address}";
        }

        public Customer Clone()
        {
            return new Customer(_service, _address);
        }
        public CustomerDTO ToDTO()
        {
            return new CustomerDTO(_service, _address);
        }
        public static Customer FromDTO(CustomerDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Customer(dto.Service, dto.Address);
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
                bool ok = Validator.TryValidateProperty(value, context, results);
                return ok ? null! : results.First().ErrorMessage!;
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
        private void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    [JsonObject(IsReference = true)]
    public class CustomerDTO
    {
        public ServiceType Service { get; set; }
        public string Address { get; set; }

        public CustomerDTO(ServiceType service, string address)
        {
            Service = service;
            Address = address;
        }
    }
}
