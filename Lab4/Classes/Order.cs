using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    [JsonObject(IsReference = true)]
    public class Order
    {
        private Executor _executor;
        private Customer _customer;
        private DateTime _orderDate;
        private int _cost;

        public Order(Executor executor, Customer customer, DateTime orderDate, int cost)
        {
            if (executor == null)
                throw new ArgumentNullException(nameof(executor), "Виконавець не може бути null.");
            if (customer == null)
                throw new ArgumentNullException(nameof(customer), "Замовник не може бути null.");
            if (orderDate > DateTime.Today)
                throw new ArgumentException("Дата замовлення не може бути у майбутньому.");
            if (cost <= 0)
                throw new ArgumentException("Вартість повинна бути додатньою.");

            _executor = executor;
            _customer = customer;
            _orderDate = orderDate.Date;
            _cost = cost;
        }

        public Executor Executor => _executor;
        public Customer Customer => _customer;
        public DateTime OrderDate => _orderDate;
        public int Cost => _cost;
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
