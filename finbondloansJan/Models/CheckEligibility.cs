using StaffLoans.LoanAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaffLoans.Models
{
    public class CheckEligibility
    {
        [Required(ErrorMessage = " ")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "ID Number")]
        [RegularExpression(@"^\d{13}$")]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Employment Type")]
        public int EmploymentTypeId { get; set; }

        public List<EmploymentType> EmploymentTypeList { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "CountryOfBirth")]
        public int CountryOfBirth { get; set; }

        public List<CountryOfBirthItem> CountryOfBirthList { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "LoanAmount")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Amount must be numeric")]
        public string LoanAmount { get; set; }

        public bool showFirstTab { get; set; }

        public bool showSecondTab { get; set; }

        public SalaryAndExpenses salaryAndExpenses { get; set; }

    }
}