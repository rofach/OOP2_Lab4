using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class ServiceBureau
    {
        private string bureauName;
        private List<Order> orders;

        public ServiceBureau(string bureauName)
        {
            if (string.IsNullOrWhiteSpace(bureauName))
                throw new ArgumentException("Назва бюро не може бути порожньою.");

            this.bureauName = bureauName;
            this.orders = new List<Order>();
        }

        public string BureauName => bureauName;
        public string ShortString => ToShortString();
        public List<Order> Orders => orders;

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            orders.Add(order);
        }
        public void AddOrder(OrderDTO order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            orders.Add(Order.FromDTO(order));
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Бюро послуг: {bureauName}");
            sb.AppendLine("Замовлення:");
            foreach (var order in orders)
            {
                sb.AppendLine(order.ToString());
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            int totalCost = orders.Sum(o => o.Cost);
            return $"Бюро: {bureauName}, Загальна вартість замовлень: {totalCost}";
        }
        public ServiceBureauDTO ToDTO()
        {
            List<OrderDTO> orderDTOs = orders.Select(o => o.ToDTO()).ToList();
            return new ServiceBureauDTO(bureauName, orderDTOs);
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
            return bureau;
        }
    }

    public class ServiceBureauDTO
    {
        public string BureauName { get; set; }
        public List<OrderDTO> Orders { get; set; }

        public ServiceBureauDTO(string bureauName, List<OrderDTO> orders)
        {
            BureauName = bureauName;
            Orders = orders;
        }
    }
}