using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
    public class Executor
    {
        private string firstName;
        private string lastName;
        private DateTime birthDate;

        public Executor(string firstName, string lastName, DateTime birthDate)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("Ім’я не може бути порожнім.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Прізвище не може бути порожнім.");
            if (birthDate > DateTime.Today)
                throw new ArgumentException("Дата народження не може бути у майбутньому.");

            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }

        public string FirstName => firstName;
        public string LastName => lastName;
        public DateTime BirthDate => birthDate;

        public override string ToString()
        {
            return $"{firstName} {lastName} ({birthDate.ToShortDateString()})";
        }
        public ExecutorDTO ToDTO()
        {
            return new ExecutorDTO(firstName, lastName, birthDate);
        }
        public static Executor FromDTO(ExecutorDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Executor(dto.FirstName, dto.LastName, dto.BirthDate);
        }

    }

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
