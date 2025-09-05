using System.ComponentModel.DataAnnotations;

namespace Project2_TipCalculator.Models
{
    public class TipCalculator
    {
        [Required(ErrorMessage = "Meal cost is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Meal cost must be greater than 0.")]
        public decimal MealCost { get; set; }

        public decimal Tip15 => MealCost * 0.15m;
        public decimal Tip20 => MealCost * 0.20m;
        public decimal Tip25 => MealCost * 0.25m;

        public decimal Total15 => MealCost + Tip15;
        public decimal Total20 => MealCost + Tip20;
        public decimal Total25 => MealCost + Tip25;
    }
}
