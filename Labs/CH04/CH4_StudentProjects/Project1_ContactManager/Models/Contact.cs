using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.RegularExpressions;

namespace Project1_ContactManager.Models
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Foreign key
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category? Category { get; set; }

        public DateTime DateAdded { get; set; }

        [NotMapped]
        public string Slug
        {
            get
            {
                var slug = $"{FirstName?.Trim()}-{LastName?.Trim()}".ToLowerInvariant();
                slug = Regex.Replace(slug, @"\s+", "-");        
                slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
                return slug;
            }
        }
    }
}
