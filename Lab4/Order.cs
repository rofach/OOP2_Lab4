using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Order
    {
        private Executor executor;
        private Customer customer;
        private DateTime orderDate;
        private int cost;

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

            this.executor = executor;
            this.customer = customer;
            this.orderDate = orderDate;
            this.cost = cost;
        }

        public Executor Executor => executor;
        public Customer Customer => customer;
        public DateTime OrderDate => orderDate;
        public int Cost => cost;
        public string OrderInfo => ToString();

        public override string ToString()
        {
            return $"Замовлення від {orderDate.ToShortDateString()}:\nВиконавець: {executor}\nЗамовник: {customer}\nВартість: {cost}";
        }
        public OrderDTO ToDTO()
        {
            return new OrderDTO(executor.ToDTO(), customer.ToDTO(), orderDate, cost);
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

    public class OrderDTO
    {
        public ExecutorDTO Executor { get; set; }
        public CustomerDTO Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public int Cost { get; set; }

        public OrderDTO(ExecutorDTO executor, CustomerDTO customer, DateTime orderDat, int cost)
        {
            Executor = executor;
            Customer = customer;
            OrderDate = orderDat;
            Cost = cost;
        }
        
    }
}
