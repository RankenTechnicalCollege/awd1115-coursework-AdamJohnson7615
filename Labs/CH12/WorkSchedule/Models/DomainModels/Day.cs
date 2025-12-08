using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace WorkSchedule.Models.DomainModels
{
    public class Day
    {

        public Day() => Positions = new HashSet<Position>();
        public int DayId { get; set; }
        [StringLength(10)]
        [Required()]
        public string Name { get; set; } = string.Empty;
        public ICollection<Position> Positions { get; set; }
    }
}
