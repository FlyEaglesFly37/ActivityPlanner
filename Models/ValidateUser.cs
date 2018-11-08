using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Belt.Models
{
    public class ValidateUser
    {
        [Key]
        public int UserId {get; set;}
        [Required(ErrorMessage = "First name is required")]
        [DataType(DataType.Text)]
        public string first_name {get; set;}
        [Required(ErrorMessage = "Last name is required")]
        [DataType(DataType.Text)]
        public string last_name {get; set;}
        [Required]
        [EmailAddress]
        public string email {get; set;}
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^.*(?=.{8,})(?=.*[a-z])(?=.*[!*@#$%^&+=]).*$", ErrorMessage="Password must have at least 1 letter, 1 number, and be at least 8 characters long")]
        public string password {get; set;}
        [Required(ErrorMessage="Please confirm password")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage="Your passwords do not match")]
        public string confirm_pw {get; set;}
    }
}