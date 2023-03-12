using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace StaffLoans.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "*")]
        [StringLength(200)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(200)]
        [Display(Name = "IDNumber")]
        public string IDNumber { get; set; }

        //[Required(ErrorMessage = "*")]
        //[StringLength(200)]
        //[Display(Name = "TwoFactorCode")]
        //public string TwoFactorCode { get; set; }

    }
}