using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaffLoans.Models
{
    public class ResetPassword
    {
        public string clientCode { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Password")]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{12}$", ErrorMessage = "Invalid Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$", ErrorMessage = "Invalid Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = " ")]
        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$", ErrorMessage = "Invalid Password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string activeReset { get; set; }
        
    }
}