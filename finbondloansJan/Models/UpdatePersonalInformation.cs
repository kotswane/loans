using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaffLoans.Models
{
    public class UpdatePersonalInformation
    {
        [Required(ErrorMessage = " ")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Original Email Address")]
        [RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$", ErrorMessage = "Invalid Email Address")]
        public string OriginalEmailAddress { get; set; }

        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^\d{10}$")]
        [Display(Name = "OriginalMobileNumber")]
        public string OriginalMobileNumber { get; set; }


        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^\d{13}$")]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Date Of Birth")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Title")]
        public int Title { get; set; }

        public List<TitleListItem> TitleList { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "CountryOfBirth")]
        public int CountryOfBirth { get; set; }

        public List<CountryOfBirthItem> CountryOfBirthList { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Nationality")]
        public int Nationality { get; set; }

        public List<NationalityItem> NationalityList { get; set; }

        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^\d{10}$")]
        [Display(Name = "MobileNumber")]
        public string MobileNumber { get; set; }
        

        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^\d{10}$")]
        [Display(Name = "WorkNumber")]
        public string WorkNumber { get; set; }

        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^\d{10}$")]
        [Display(Name = "AlternateNumber")]
        public string AlternateNumber { get; set; }


        [Required(ErrorMessage = " ")]
        [Display(Name = "Language")]
        public int Language { get; set; }

        public List<LanguageItem> LanguageList { get; set; }


        [Required(ErrorMessage = " ")]
        [Display(Name = "Alternate Contact First Name")]
        public string AlternateContactFirstName { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Alternate Contact Last Name")]
        public string AlternateContactLastName { get; set; }


        [Required(ErrorMessage = " ")]
        [RegularExpression(@"^\d{10}$")]
        [Display(Name = "Alternate Contact Number")]
        public string AlternateContactNumber { get; set; }


        [Required(ErrorMessage = " ")]
        //[RegularExpression(@"^\d{10}$")]
        [Range(1, 20000, ErrorMessage = "Value must be between 1 to 20000")]
        [Display(Name = "Loan Amount")]
        public string LoanAmount { get; set; }



        [Required(ErrorMessage = " ")]
        [Display(Name = "Marketing Consent")]
        public bool MarketingConsent { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Marketing Preference")]
        public int MarketingPreference { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Employment Type")]
        public int EmploymentTypeId { get; set; }


        public List<EmploymentType> EmploymentTypeList { get; set; }

        public List<MarketingPreferenceItem> MarketingPreferenceList { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Marital Status")]
        public int MaritalStatus { get; set; }

        public List<MaritalStatusItem> MaritalStatusList { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Email Address")]
        [RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$", ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }
        

        [Required(ErrorMessage = " ")]
        [Display(Name = "Password")]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{12}$", ErrorMessage = "Invalid Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,}$", ErrorMessage = "Invalid Password")]

        public string Password { get; set; }

        public decimal YourSalary { get; set; }
        [Required(ErrorMessage = " ")]
        [RegularExpression(@"((([R ]+)?)(\d+)((\.\d{1,2})?))$")]
        [Display(Name = "Your Salary")]
        public string YourSalaryString { get; set; }



        #region Residential Address
        [Required(ErrorMessage = " ")]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Suburb")]
        public string Suburb { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        public List<CountryItem> CountryList { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Province")]
        public string Province { get; set; }

        public List<ProvinceItem> ProvinceList { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "PostalCode")]
        public string PostalCode { get; set; }
        #endregion

        #region PostalAddress
        [Required(ErrorMessage = " ")]
        [Display(Name = "PostalStreet")]
        public string PostalStreet { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "PostalSuburb")]
        public string PostalSuburb { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "PostalCountry")]
        public string PostalCountry { get; set; }



        [Required(ErrorMessage = " ")]
        [Display(Name = "PostalProvince")]
        public string PostalProvince { get; set; }



        [Required(ErrorMessage = " ")]
        [Display(Name = "PostalPostalCode")]
        public string PostalPostalCode { get; set; }
        #endregion

        public string ClientCode { get; set; }

    }
}