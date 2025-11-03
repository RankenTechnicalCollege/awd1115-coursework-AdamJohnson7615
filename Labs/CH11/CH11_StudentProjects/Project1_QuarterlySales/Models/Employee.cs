using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1_QuarterlySales.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        [PastDate(ErrorMessage = "Date of Birth must be in the past.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Hire")]
        [PastDate(ErrorMessage = "Date of Hire must be in the past.")]
        [HireDate(ErrorMessage = "Date of Hire cannot be before 1/1/1995.")]
        public DateTime DateOfHire { get; set; }

        [Required]
        public int? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        [Display(Name = "Manager")]
        public Employee? Manager { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
