using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CBT.Models
{
    public class NewUser : BaseEntity
    {
        [Required]
        [Display(Name="First Name")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name="Last Name")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space")]
        public string LastName { get; set; }
 
        [Required]
        [MinLength(3, ErrorMessage = "Must be at least two characters.")]
        [MaxLength(20, ErrorMessage = "Cannot be longer than twenty characters.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space")]
        [Display(Name="Username")]
        public string Username { get; set; }
 
        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Must be at least eight characters.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space")]
        [Display(Name="Password")]
        public string PW { get; set; }
 
        [Compare("PW", ErrorMessage = "Password and confirmation must match.")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        public string CPW { get; set; }
    }

        public class LogUser : BaseEntity
    {
        [Required]
        [Display(Name="Username")]
        public string LogUsername {get;set;}
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string LogPW {get;set;}
    }
    
}
