using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab4
{
    [JsonObject(IsReference = true)]
    public class Order : IDataErrorInfo, INotifyPropertyChanged
    {
        private Executor? _executor;
        private Customer? _customer;
        private DateTime _orderDate;
        private int _cost;
        public Order() 
        {
            _orderDate = DateTime.Today;
        }

        public Order(Executor executor, Customer customer, DateTime orderDate, int cost)
        {
            /*if (executor == null)
                throw new ArgumentNullException(nameof(executor), "Виконавець не може бути null.");
            if (customer == null)
                throw new ArgumentNullException(nameof(customer), "Замовник не може бути null.");
            if (orderDate > DateTime.Today)
                throw new ArgumentException("Дата замовлення не може бути у майбутньому.");
            if (cost <= 0)
                throw new ArgumentException("Вартість повинна бути додатньою.");*/

            _executor = executor;
            _customer = customer;
            _orderDate = orderDate.Date;
            _cost = cost;
        }
        [Required(ErrorMessage = "виконавець обов'язкова")]
        public Executor Executor
        {
            get => _executor;
            set { _executor = value; OnPropertyChanged(nameof(Executor)); }
        }
        [Required(ErrorMessage = "замовник обов'язкова")]
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value; OnPropertyChanged(nameof(Customer));
            }
        }
        [Required(ErrorMessage = "Час обов'язкова")]
        public DateTime OrderDate
        {
            get => _orderDate;
            set { _orderDate = value.Date; OnPropertyChanged(nameof(OrderDate)); }
        }
        [Required(ErrorMessage = "Адреса обов'язкова")]
        [Range(0, 10000, ErrorMessage = "Не в діапазоні")]
        public int Cost
        {
            get => _cost;
            set { _cost = value; OnPropertyChanged(nameof(Cost)); }
        }

        public string OrderInfo => $"{_executor} {_orderDate.ToShortDateString()}";

        public override string ToString()
        {
            return $"Замовлення від {_orderDate.ToShortDateString()}:\nВиконавець: {_executor}\nЗамовник: {_customer}\nВартість: {_cost}";
        }
        public OrderDTO ToDTO()
        {
            return new OrderDTO(_executor.ToDTO(), _customer.ToDTO(), _orderDate, _cost);
        }
        public static Order FromDTO(OrderDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            Executor executor = Executor.FromDTO(dto.Executor);
            Customer customer = Customer.FromDTO(dto.Customer);
            return new Order(executor, customer, dto.OrderDate, dto.Cost);
        }

        public string this[string columnName]
        {
            get
            {
                var prop = GetType().GetProperty(columnName);
                if (prop == null) return null!;
                var value = prop.GetValue(this);
                var ctx = new ValidationContext(this) { MemberName = columnName };
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                bool ok = Validator.TryValidateProperty(value, ctx, results);
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
                return string.Join(Environment.NewLine, results.Select(r => r.ErrorMessage));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

    }


    [JsonObject(IsReference = true)]
    public class OrderDTO
    {
        public ExecutorDTO Executor { get; set; }
        public CustomerDTO Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public int Cost { get; set; }

        public OrderDTO(ExecutorDTO executor, CustomerDTO customer, DateTime orderDate, int cost)
        {
            Executor = executor;
            Customer = customer;
            OrderDate = orderDate.Date;
            Cost = cost;
        }

    }
}
