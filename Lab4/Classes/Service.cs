using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab4
{
    public enum ServiceType
    {
        [Description("Прибирання")]
        Cleaning,

        [Description("Миття вікон")]
        WindowWashing,

        [Description("Догляд за дитиною")]
        ChildCare,

        [Description("Дрібний ремонт")]
        MinorRepairs,

        [Description("Змішана")]
        Mixed
    }

    public static class EnumDescription
    {
        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0) return attributes[0].Description;
            else return value.ToString();
        }
    }
}
