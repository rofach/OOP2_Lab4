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
    public class Executor
    {
        private string _firstName;
        private string _lastName;
        private DateTime _birthDate;

        public Executor(string firstName, string lastName, DateTime birthDate)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ім’я не може бути порожнім.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Прізвище не може бути порожнім.");
            if (birthDate > DateTime.Today)
                throw new ArgumentException("Дата народження не може бути у майбутньому.");

            _firstName = firstName;
            _lastName = lastName;
            _birthDate = birthDate;
        }

        public string FirstName => _firstName;
        public string LastName => _lastName;
        public DateTime BirthDate => _birthDate;
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
