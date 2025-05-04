using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab4.Classes.ValidationAttributes
{
    public class NameValidation : ValidationAttribute
    {
        private const string _pattern = @"\p{L}+(?:[ -]\p{L}+)*$";
        private static readonly Regex _regex = new Regex(_pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
        public override bool IsValid(object? value)
        {
            string input = value as string;
            if (!_regex.IsMatch(input))
            {
                return false;
            }
            return true;
        }
    }
}
