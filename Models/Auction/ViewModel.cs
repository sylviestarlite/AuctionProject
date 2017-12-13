using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CBT.Models
{
    public class Dashboard
    {
        public List<Auction> Auctions {get;set;}
        public User User {get;set;}
    }

    public class ViewAuction
    {
        [Required]
        [MinLength(3, ErrorMessage = "Must be at least three characters")]
        [Display(Name="Name")]

        public string Name {get;set;}

        [Required]
        [MinLength(10, ErrorMessage = "Must be at least ten characters")]
        [Display(Name="Description")]
        public string Description {get;set;}

        [Required(ErrorMessage = "Please enter a date")]
        [FutureDate]
        [DataType(DataType.Date)]
        [Display(Name="End Date")]
        public DateTime Date {get;set;}

        [Required(ErrorMessage = "Please enter a bid")]
        [Range(1, float.MaxValue, ErrorMessage = "Please enter a value larger than 0")]
        [Display(Name="Starting Bid")]
        public float StartingBid {get;set;}
    }

    public class FutureDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value;
            return date < DateTime.Now ? new ValidationResult("Date must be in the future") : ValidationResult.Success;
        }
    }

    public class ViewBid
    {
        [Required]
        [Display(Name="Your bid")]

        public string Bid {get;set;}
    }
}