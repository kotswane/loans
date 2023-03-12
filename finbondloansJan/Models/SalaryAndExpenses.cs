using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaffLoans.Models
{
    public class MyAccount
    {
        public string applicationNumber { get; set; }
        public string selectedLoanId { get; set; }
        public string selectedOfferId { get; set; }

        //public string AccountType { get; set; }

        //public string Bank{ get; set; }
        //public string BranchCode { get; set; }
        public int ExpectedNumberOfDocumentUploads { get; set; }
        public SalaryAndExpenses salaryAndExpenses { get; set; }
        public EmploymentDet employmentDetails { get; set; }
        public BankDet bankDetails { get; set; }
        public List<DocumentTypes> DocumentTypes { get; set; }
        public List<AccountTypes> AccountTypes { get; set; }

        public List<BanksList> BankList { get; set; }

        public List<CountryItem> CountryList { get; set; }
        public List<ProvinceItem> ProvinceList { get; set; }

        public List<OccupationTypeList > occupationTypes { get; set; }
        public List<PayFrequencyList > payFrequencyList { get; set; }
        public List<SalaryMethodList > salaryMethodList { get; set; }
        public List<RepaymentMethodList> repaymentMethodList { get; set; }
        public List<PaydayShiftList> paydayShiftList { get; set; }
        //public List<BranchCodeList> BranchCodeList { get; set; }
    }

    public class EmploymentDet
    {
        
        public string EmployerClientCode { get; set; }

        [Required(ErrorMessage = " ")]
        public string EmployerName { get; set; }

        [Required(ErrorMessage = " ")]
        public string Department { get; set; }

        [Required(ErrorMessage = " ")]
        public string Placement { get; set; }

        [Required(ErrorMessage = " ")]
        public string Occupation { get; set; }

        [Required(ErrorMessage = " ")]
        public string OccupationType { get; set; }

        [Required(ErrorMessage = " ")]
        public string EmployeeNumber { get; set; }

        [Required(ErrorMessage = " ")]
        public string EmployerContactNumber { get; set; }

        [Required(ErrorMessage = " ")]
        public string AppointmentDate { get; set; }

        [Required(ErrorMessage = " ")]
        public string PayFrequency { get; set; }

        [Required(ErrorMessage = " ")]
        public string SalaryMethod { get; set; }

        [Required(ErrorMessage = " ")]
        public string Paydayshift { get; set; }

        [Required(ErrorMessage = " ")]
        public string RepaymentMethod { get; set; }

        [Required(ErrorMessage = " ")]
        public string ContractEndDate { get; set; }

        [Required(ErrorMessage = " ")]
        public string Street { get; set; }

        [Required(ErrorMessage = " ")]
        public string Suburb { get; set; }

        [Required(ErrorMessage = " ")]
        public string Province { get; set; }

        [Required(ErrorMessage = " ")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = " ")]
        public string Country { get; set; }



    }

    public class BankDet
    {
        public string BankClientCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string Bank { get; set; }

    }
    public class ReasonForLoan
    {
        public int? ReasonId { get; set; }
        public string ReasonDesc { get; set; }
    }
    public class OccupationTypeList
    {
        public string OccupationTypeId { get; set; }
        public string OccupationTypeDesc { get; set; }
    }

    public class PayFrequencyList
    {
        public string PayFrequencyId { get; set; }
        public string PayFrequencyDesc { get; set; }
    }

    public class SalaryMethodList
    {
        public string SalaryMethodID { get; set; }
        public string SalaryMethodDesc { get; set; }
    }

    public class RepaymentMethodList
    {
        public string RepaymentMethodId { get; set; }
        public string RepaymentMethodDesc { get; set; }
    }

    public class PaydayShiftList
    {
        public string PaydayshiftId { get; set; }
        public string PaydayShiftDesc { get; set; }
    }


    public class BanksList
    {
        public string BankId { get; set; }
        public string BankDesc { get; set; }
    }
    public class BranchCodeList
    {
        public string BranchCodeId { get; set; }
        public string BranchCodes { get; set; }
    }

    public class AccountTypes
    {
        public string AccountTypeId { get; set; }
        public string AccountTypeDesc { get; set; }
    }
    public class DocumentTypes
    {
        public string DocumentTypeId { get; set; }
        public string DocumentTypeDesc { get; set; }
    }

    public class DebitOrderDateList
    {
        public string DebitOrderValue { get; set; }
        public string DebitOrderText { get; set; }
    }
    public class SalaryAndExpenses
    {
        [Required(ErrorMessage = " ")]
        [Display(Name = "Last Month Salary")]
        public string   LastMonthSalary { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Last Second MonthSalary")]
        public string LastTwoMonthSalary { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Last Third MonthSalary")]
        public string LastThreeMonthSalary { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Groceries")]
        public string Groceries { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Transport")]
        public string Transport { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Housing")]
        public string Housing { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Clothing")]
        public string Clothing { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Medical")]
        public string Medical { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "WaterElectricty")]
        public string WaterElectricty { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "ChildMaintainance")]
        public string ChildMaintainance { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Education")]
        public string Education { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "BankCharges")]
        public string BankCharges { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Other")]
        public string Other { get; set; }

        //[Required(ErrorMessage = " ")]
        [Display(Name = "ReasonForLoan")]
        public int ReasonForLoan { get; set; }

        public int DebitOrderDate { get; set; }

        public List<DebitOrderDateList> DebitOrderDateList;

        public List<ReasonForLoan> ReasonForLoanList;



        public string documentInnerHTML { get; set; }
    }
}