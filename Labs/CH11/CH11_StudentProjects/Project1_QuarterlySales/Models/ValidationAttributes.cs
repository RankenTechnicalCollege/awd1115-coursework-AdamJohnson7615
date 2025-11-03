using System;
using System.ComponentModel.DataAnnotations;

namespace Project1_QuarterlySales.Models
{
    public class PastDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
                return date < DateTime.Today;
            return true;
        }
    }

    public class HireDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
                return date >= new DateTime(1995, 1, 1);
            return true;
        }
    }
}
