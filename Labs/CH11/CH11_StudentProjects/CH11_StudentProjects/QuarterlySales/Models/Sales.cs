using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QuarterlySales.Data;

namespace QuarterlySales.Models
{
    public class Sales : IValidatableObject
    {
        public int SalesId { get; set; }

        [Required(ErrorMessage = "Quarter is required.")]
        [Range(1, 4, ErrorMessage = "Quarter must be between 1 and 4.")]
        public int Quarter { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Range(2001, int.MaxValue, ErrorMessage = "Year must be after 2000.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = (AppDbContext)validationContext.GetService(typeof(AppDbContext));

            // Unique sales record for same quarter, year, and employee
            if (db.Sales.Any(s => s.Quarter == Quarter && s.Year == Year && s.EmployeeId == EmployeeId && s.SalesId != SalesId))
                yield return new ValidationResult("Sales data for this employee for the same quarter and year already exists.");
        }
    }
}
