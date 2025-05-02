using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    [JsonObject(IsReference = true)]
    public class Customer
    {
        private ServiceType _service;
        private string _address;

        public Customer(ServiceType service, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Адреса не може бути порожньою.");

            _service = service;
            _address = address;
        }

        public ServiceType Service => _service;
        public string Address => _address;

        public override string ToString()
        {
            return $"Послуга: {_service}, Адреса: {_address}";
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
