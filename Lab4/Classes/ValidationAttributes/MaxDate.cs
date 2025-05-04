using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Classes.ValidationAttributes
{
    public class MaxDate : ValidationAttribute
    {
        private readonly DateTime _maxDate;

        public MaxDate()
        {
            _maxDate = DateTime.Today;
        }
        public MaxDate(int year, int month, int day)
        {
            _maxDate = new DateTime(year, month, day);
        }
     

        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date <= _maxDate;
            }
            return false;
        }


    }
    
    
}
