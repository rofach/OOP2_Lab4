using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    [JsonObject(IsReference = true)]
    public class ServiceBureau
    {
        [Length(1, 50, ErrorMessage = "Назва бюро повинна бути від 1 до 50 символів.")]
        private string _bureauName;
        private List<Order> _orders;
        private List<Executor> _executors;
        public ServiceBureau(string bureauName)
        {
            if (string.IsNullOrWhiteSpace(bureauName))
                throw new ArgumentException("Назва бюро не може бути порожньою.");

            _bureauName = bureauName;
            _orders = new List<Order>();
            _executors = new List<Executor>();
        }

        public string BureauName
        {
            get => _bureauName;
            private set => _bureauName = value;
        }
        public string ShortString => ToShortString();
        public List<Order> Orders
        {
            get => _orders;
            set => _orders = value;
        }
        public List<Executor> Executors { 
            get { return _executors; } 
            set { _executors = value; } }

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            _orders.Add(order);
        }
      
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Бюро послуг: {_bureauName}");
            sb.AppendLine("Замовлення:");
            foreach (var order in _orders)
            {
                sb.AppendLine(order.ToString());
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            int totalCost = _orders.Sum(o => o.Cost);
            return $"Бюро: {_bureauName}, Загальна вартість замовлень: {totalCost}";
        }
        public ServiceBureau Clone()
        {
            return new ServiceBureau(_bureauName)
            {
                Orders = _orders.Select(o => o.Clone()).ToList(),
                Executors = _executors.ToList()

            };
        }
            public ServiceBureauDTO ToDTO()
        {
            List<OrderDTO> orderDTOs = _orders.Select(o => o.ToDTO()).ToList();
            List<ExecutorDTO> executorDTOs = _executors.Select(e => e.ToDTO()).ToList();
            return new ServiceBureauDTO(_bureauName, orderDTOs, executorDTOs);
        }
       

        public static ServiceBureau FromDTO(ServiceBureauDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            ServiceBureau bureau = new ServiceBureau(dto.BureauName);
            foreach (var orderDto in dto.Orders)
            {
                bureau.AddOrder(Order.FromDTO(orderDto));
            }
            foreach (var executorDto in dto.Executors)
            {
                bureau.Executors.Add(Executor.FromDTO(executorDto));
            }
            return bureau;
        }
    }
    [JsonObject(IsReference = true)]
    public class ServiceBureauDTO
    {
        public string BureauName { get; set; }
        public List<OrderDTO> Orders { get; set; }
        public List<ExecutorDTO> Executors { get; set; }
        public ServiceBureauDTO(string bureauName, List<OrderDTO> orders, List<ExecutorDTO> executors)
        {
            BureauName = bureauName;
            Orders = orders;
            Executors = executors;
        }
    }
}