using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAppCore.Models
{
    public class SignUpUserModel
    {
        [Required,Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }
        [Required, Display(Name = "Email address"),EmailAddress(ErrorMessage ="Please enter valid email address")]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, Display(Name = "Confirm Password"),Compare("Password",ErrorMessage ="Password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
