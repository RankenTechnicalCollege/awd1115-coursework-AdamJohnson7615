using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DocStop.Models
{
    public class Appointment : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Start date/time is required.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date/Time")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Customer is required.")]
        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Start.Minute != 0 || Start.Second != 0 || Start.Millisecond != 0)
            {
                yield return new ValidationResult(
                    "Appointments must start exactly on the hour (for example: 2025-03-15 08:00).",
                    new[] { nameof(Start) });
            }

            if (Start <= DateTime.Now)
            {
                yield return new ValidationResult(
                    "Appointment start time must be in the future.",
                    new[] { nameof(Start) });
            }

            var db = (Data.AppDbContext?)validationContext.GetService(typeof(Data.AppDbContext));
            if (db != null)
            {
                var exists = db.Appointments.AsNoTracking()
                    .Any(a => a.Start == Start && a.Id != this.Id);

                if (exists)
                {
                    yield return new ValidationResult(
                        $"The appointment slot ({Start:yyyy-MM-dd HH:mm}) is already taken. Please choose another hour.",
                        new[] { nameof(Start) });
                }
            }
            else
            {
               
            }
        }
    }
}
