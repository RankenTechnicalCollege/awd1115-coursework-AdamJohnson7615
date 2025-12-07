using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using QuarterlySales.Data;

namespace QuarterlySales.Models
{
    public class Employee : IValidatableObject
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Date of hire is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfHire { get; set; }

        [Required(ErrorMessage = "Manager is required.")]
        public int ManagerId { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = (AppDbContext)validationContext.GetService(typeof(AppDbContext));

            if (DOB >= DateTime.Now)
                yield return new ValidationResult("Date of birth must be in the past.", new[] { nameof(DOB) });

            if (DateOfHire >= DateTime.Now)
                yield return new ValidationResult("Date of hire must be in the past.", new[] { nameof(DateOfHire) });

            if (DateOfHire < new DateTime(1995, 1, 1))
                yield return new ValidationResult("Date of hire cannot be before 1/1/1995.", new[] { nameof(DateOfHire) });

            if (db.Employees.Any(e => e.FirstName == FirstName && e.LastName == LastName && e.DOB == DOB && e.EmployeeId != EmployeeId))
                yield return new ValidationResult("An employee with the same first name, last name, and date of birth already exists.");

            if (ManagerId == EmployeeId)
                yield return new ValidationResult("Employee cannot be their own manager.");
        }
    }
}
