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
    public class ForgotPasswordStep1
    {
        [Required(ErrorMessage = " ")]
        [StringLength(200)]
        [Display(Name = "IDNumber")]
        public string IDNumber { get; set; }
    }

    public class ForgotPasswordStep2
    {
        [Required(ErrorMessage = " ")]
        [StringLength(200)]
        [Display(Name = "OTP")]
        public string OTP { get; set; }
    }

    public class ForgotPassword
    {
        public ForgotPasswordStep1 Step1 { get; set; }

        public ForgotPasswordStep2 Step2 { get; set; }
        //[Required(ErrorMessage = "*")]
        //[StringLength(200)]
        //[Display(Name = "TwoFactorCode")]
        //public string TwoFactorCode { get; set; }

        public int sentStatus { get; set; }
    }
}