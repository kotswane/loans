using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaffLoans.Models
{
    public class LoanInfo
    {
        [Required(ErrorMessage = " ")]
        [Display(Name = "ContractNo")]
        public string ContractNo { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "ClientNo")]
        public string ClientNo { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "ContractDate")]
        public string ContractDate { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "LoanAmt")]
        public decimal  LoanAmt { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "InstallmentsRemaining")]
        public int InstallmentsRemaining { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "NoOfInstallmentsPaid")]
        public int NoOfInstallmentsPaid { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "AmountInArrears")]
        public decimal  AmountInArrears { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "RemainingBalance")]
        public decimal RemainingBalance { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "NextInstallment")]
        public decimal NextInstallment { get; set; }

       

        [Required(ErrorMessage = " ")]
        [Display(Name = "LastPayDate")]
        public string LastPayDate { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "NextInstallmentDate")]
        public string NextInstallmentDate { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "NoOfInstallments")]
        public int NoOfInstallments { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "EasypayRefNumber")]
        public string EasypayRefNumber { get; set; }
        //[Required(ErrorMessage = " ")]
        //[Display(Name = "OutstandingAmt")]
        //public string OutstandingAmt { get; set; }

        public List<PaymentHistory> lstPayments { get; set; }
        public string arrearsCssClass { get; set; }

        public decimal  outstandingamt { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "LoanStatus")]
        public string LoanStatus
        {
            get
            {
                if (outstandingamt == 0)
                    return "Paid Up";
                else if (AmountInArrears > 0)
                    return "Past Due";
                else
                    return "Current";
            }
        }

        
        [Display(Name = "ImgName")]
        public string ImgName {
            get
            {
                return NoOfInstallments + "_" + NoOfInstallmentsPaid + ".png";
            }
        }

        public string monthsPadCssClass { get; set; }
    }

    public class PaymentHistory
    {
        public string MonthName { get; set; }
        public bool IsPaid { get; set; }
    }
}