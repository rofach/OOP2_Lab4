using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Customer
    {
        private ServiceType service;
        private string address;

        public Customer(ServiceType service, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Адреса не може бути порожньою.");

            this.service = service;
            this.address = address;
        }

        public ServiceType Service => service;
        public string Address => address;

        public override string ToString()
        {
            return $"Послуга: {service}, Адреса: {address}";
        }
        public CustomerDTO ToDTO()
        {
            return new CustomerDTO(service, address);
        }
        public static Customer FromDTO(CustomerDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Customer(dto.Service, dto.Address);
        }
    }
    public class CustomerDTO
    {
        public ServiceType Service { get; set; }
        public string Address { get; set; }

        public CustomerDTO(ServiceType service, string address)
        {
            //if (string.IsNullOrWhiteSpace(address))
            //    throw new ArgumentException("Адреса не може бути порожньою.");

            Service = service;
            Address = address;
        }
    }
}
