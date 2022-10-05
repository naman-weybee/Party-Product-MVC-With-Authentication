using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartyProduct_Exercise_03.Models
{
    public class SignUpUserModel
    {
        [Required(ErrorMessage = "Please Enter your FirstName")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter your Email")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Strong Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please Confirm your Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
