using StaffLoans.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EvoPdf.HtmlToPdf;
using System.Text;
using log4net;
using Newtonsoft.Json;

namespace StaffLoans.Controllers
{
    public class MyAccountController :BaseController 
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");
        private static int ExpectedNumberOfDocumentUploads { get; set; }

        // [CustomAuthorize]
        public ActionResult Index()
        {
            Common.UploadedDocumentList = new List<Document>();
            //SendEmailToHR("laxmi.guntuka", "Laxmi");
            //StaffLoans.Models.Common.LoggedInClientIDNumber = "8606131285187";
            //StaffLoans.Models.Common.LoggedInClientUSN = "0";
            MyAccount mod = new Models.MyAccount();
            mod.employmentDetails = new Models.EmploymentDet();
            mod.bankDetails = new BankDet();
            try
            {
                LoanAPI.TConnectionCheckReply conReply = new LoanAPI.TConnectionCheckReply();
                LoanAPI.TClientEnquiryReply clientReply = new LoanAPI.TClientEnquiryReply();
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    //wLogger.Info("OperatorLogon request");
                    //var a = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                    //wLogger.Info("OperatorLogon response" + JsonConvert.SerializeObject(a));
                    wLogger.Info("ClientEnquiry request");
                    clientReply = proxy.ClientEnquiry(Common.getClientSessionData(), Common.LoggedInClientIDNumber, false);
                    wLogger.Info("ClientEnquiry response" + JsonConvert.SerializeObject(clientReply));
                    conReply = proxy.ConnectionCheck(1, 1);
                   // var r= proxy.InsUpdEmployerDetails ()
                }
                if (clientReply != null)
                {
                    if(clientReply.EmployerData !=null )
                    {
                        mod.employmentDetails.EmployerClientCode = clientReply.EmployerData.EmployerClientCode.ToString();
                        mod.employmentDetails.AppointmentDate = clientReply.EmployerData.AppointedOn.ToString();
                        mod.employmentDetails.ContractEndDate = clientReply.EmployerData.ContractEndDate.ToString();
                        mod.employmentDetails.Country = clientReply.EmployerData.EmployerCountry;
                        mod.employmentDetails.Department = clientReply.EmployerData.Department ;
                        mod.employmentDetails.EmployeeNumber = clientReply.EmployerData.EmployeeNumber ;
                        mod.employmentDetails.EmployerContactNumber = clientReply.EmployerData.EmployerContactNumber ;
                        mod.employmentDetails.EmployerName = clientReply.EmployerData.Employer ;
                        mod.employmentDetails.Occupation = clientReply.EmployerData.Occupation ;
                        mod.employmentDetails.OccupationType = clientReply.EmployerData.OccupationType.ToString();
                        mod.employmentDetails.Paydayshift = clientReply.EmployerData.PayDayShift.ToString();
                        mod.employmentDetails.PayFrequency = clientReply.EmployerData.PayFrequency.ToString();
                        mod.employmentDetails.Placement = clientReply.EmployerData.Placement;
                        mod.employmentDetails.PostalCode = clientReply.EmployerData.EmployerPostalCode.ToString();
                        mod.employmentDetails.Province = clientReply.EmployerData.EmployerProvince ;
                        mod.employmentDetails.RepaymentMethod = clientReply.EmployerData.RepayMethod.ToString();
                        mod.employmentDetails.SalaryMethod = clientReply.EmployerData.SalaryMethod.ToString();
                        mod.employmentDetails.Street = clientReply.EmployerData.EmployerStreet ;
                        mod.employmentDetails.Suburb = clientReply.EmployerData.EmployerSuburb ;
                    }
                    if (clientReply.BankingData  != null)
                    {
                        mod.bankDetails.BankClientCode= clientReply.BankingData.BankingClientCode.ToString();
                        mod.bankDetails.AccountNumber =clientReply.BankingData.AccountNumber;
                        mod.bankDetails.AccountType = clientReply.BankingData.AccountType.ToString() ;
                        mod.bankDetails.Bank = clientReply.BankingData.BranchCode;

                    }
                 }
                mod.salaryAndExpenses = new SalaryAndExpenses();
                if (conReply != null)
                {
                    if (conReply.LookupValues != null)
                    {
                        #region DOCUMENT TYPES
                        //var docTypes = conReply.LookupValues.Where(m => m.LookupName == "DOCUMENT TYPE");

                        mod.DocumentTypes = (from reqDocs in conReply.ClientTypeRequiredDocuments.Where(m => m.ClientType == Convert.ToInt32( ConfigurationManager.AppSettings["ClientType"] ))
                                             join dt in conReply.LookupValues.Where(m => m.LookupName == "DOCUMENT TYPE")
                                             on reqDocs.DocumentType equals uint.Parse(dt.Code.ToString())
                                             select new DocumentTypes()
                                             {
                                                 DocumentTypeId = reqDocs.DocumentType.ToString(),
                                                 DocumentTypeDesc = dt.Description
                                             }
                                             ).ToList();

                        #region add id document and selfie document
                        mod.DocumentTypes.Add(new DocumentTypes()
                        {
                            DocumentTypeId ="8",
                            DocumentTypeDesc ="Selfie"
                        });
                        mod.DocumentTypes.Add(new DocumentTypes()
                        {
                            DocumentTypeId = "9",
                            DocumentTypeDesc = "ID"
                        });
                        if (mod.DocumentTypes.Exists(m => m.DocumentTypeId == "4"))
                        {
                            var docToRemove = mod.DocumentTypes.First(m => m.DocumentTypeId == "4");
                            mod.DocumentTypes.Remove(docToRemove);
                        }
                        #endregion
                        ExpectedNumberOfDocumentUploads = mod.DocumentTypes.Count;
                        mod.ExpectedNumberOfDocumentUploads = mod.DocumentTypes.Count;
                        #endregion

                        #region Account Types
                        mod.AccountTypes  = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "ACCOUNT TYPE").ToList()
                                                select new AccountTypes()
                                                {
                                                    AccountTypeId = r.Code.ToString(),
                                                    AccountTypeDesc = r.Description
                                                }
                                          ).ToList();
                        #endregion

                        #region Banks
                        mod.BankList = new List<Models.BanksList>();
                        List<Models.BanksList> blist = new List<BanksList>();

                        BanksList b = new BanksList();
                        b.BankId = "FNB";
                        b.BankDesc = "FNB";
                        blist.Add(b);

                        BanksList b2 = new BanksList();
                        b2.BankId = "ABSA";
                        b2.BankDesc = "ABSA";
                        blist.Add(b2);
                        mod.BankList = blist;
                        #endregion

                        #region BranchCode
                        //mod.BranchCodeList = new List<Models.BranchCodeList>();
                        #endregion

                        #region Country
                        CountryItem citem = new CountryItem();
                        citem.CountryId = "South Africa";
                        citem.CountryDesc = "South Africa";
                        mod.CountryList = new List<Models.CountryItem>();
                        mod.CountryList.Add(citem);

                        #endregion

                        #region province
                        mod.ProvinceList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "PROVINCE").ToList()
                                                select new ProvinceItem()
                                                {
                                                    ProvinceId = r.Code,
                                                    ProvinceDesc = r.Description
                                                }
                                          ).ToList();

                        #endregion

                        #region OccupationType
                        mod.occupationTypes  = (
                                              from r in conReply.LookupValues.Where(m => m.LookupName == "OCCUPATION TYPE").ToList()
                                              select new OccupationTypeList()
                                              {
                                                  OccupationTypeId  = r.Code.ToString(),
                                                  OccupationTypeDesc  = r.Description
                                              }
                                        ).ToList();
                        #endregion

                        #region Payfrequency
                        mod.payFrequencyList = (
                                              from r in conReply.LookupValues.Where(m => m.LookupName == "PAY FREQUENCY").ToList()
                                              select new PayFrequencyList()
                                              {
                                                  PayFrequencyId  = r.Code.ToString(),
                                                  PayFrequencyDesc  = r.Description
                                              }
                                        ).ToList();
                        #endregion

                        #region SalaryMethod
                        mod.salaryMethodList  = (
                                 from r in conReply.LookupValues.Where(m => m.LookupName == "SALARY METHOD").ToList()
                                 select new SalaryMethodList ()
                                 {
                                     SalaryMethodID = r.Code.ToString(),
                                     SalaryMethodDesc = r.Description
                                 }
                           ).ToList();
                        #endregion

                        #region RepaymentMethod
                        mod.repaymentMethodList = (
                                from r in conReply.LookupValues.Where(m => m.LookupName == "REPAY METHOD").ToList()
                                select new RepaymentMethodList()
                                {
                                    RepaymentMethodId  = r.Code.ToString(),
                                    RepaymentMethodDesc  = r.Description
                                }
                          ).ToList();
                        #endregion

                        #region Paydayshift
                        mod.paydayShiftList  = (
                          from r in conReply.LookupValues.Where(m => m.LookupName == "PAY DAY SHIFT").ToList()
                          select new PaydayShiftList ()
                          {
                              PaydayshiftId = r.Code.ToString(),
                              PaydayShiftDesc  = r.Description
                          }
                    ).ToList();
                        #endregion

                        #region reaonForLoaan
                        mod.salaryAndExpenses.ReasonForLoanList = (
                                                      from r in conReply.LookupValues.Where(m => m.LookupName == "REASON FOR LOAN").ToList()
                                                      select new ReasonForLoan()
                                                      {
                                                          ReasonId = r.Code,
                                                          ReasonDesc = r.Description
                                                      }
                                                ).ToList();
                        #endregion

                        #region debitorderdate
                        int startInXDays = conReply.DebitOrderStartWindow ;
                        int stopInXDaysFromStart = conReply.DebitOrderDaysToShow ;

                        DateTime fromDate = DateTime.Now.AddDays(startInXDays);
                        DateTime toDate = DateTime.Now.AddDays(startInXDays + stopInXDaysFromStart - 1);
                        List<DebitOrderDateList> srtDates = new List<DebitOrderDateList>();
                        DateTime dtTemp = fromDate;
                        while (dtTemp <= toDate)
                        {
                            DebitOrderDateList d = new DebitOrderDateList();
                            d.DebitOrderValue = dtTemp.ToString("yyyyMMdd");
                            d.DebitOrderText = dtTemp.ToString("yyyy/MM/dd");
                            srtDates.Add(d);
                            dtTemp = dtTemp.AddDays(1);
                        }
                        mod.salaryAndExpenses.DebitOrderDateList = srtDates;
                        #endregion
                    }
                }

               
                //************************view contract*************************************
                var viewContractPath = ConfigurationManager.AppSettings["ViewContractPath"].ToString();
                if (System.IO.File.Exists(HttpContext.Server.MapPath(viewContractPath)))
                {
                    StringWriter sw = new StringWriter();
                    System.Web.HttpContext.Current.Server.Execute(viewContractPath, sw);
                    string htmlCodeToConvert = sw.GetStringBuilder().ToString();
                    DateTimeFormatInfo mfi = new DateTimeFormatInfo();

                    htmlCodeToConvert = htmlCodeToConvert.Substring(3).Replace("[FIRSTNAME]", "" + StaffLoans.Models.Common.LoggedInClientName + "").Replace("[DAY]", "" + DateTime.Today.Day + "").Replace("[MONTH]", "" + mfi.GetMonthName(DateTime.Today.Month) + "").Replace("[YEAR]", "" + DateTime.Today.Year + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[SIGNEDAT]", "Rosebank");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[SURNAME]", "" + Common.LoggedInClientSurName + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[PHYSICALADDRESS]", "" + Common.LoggedInClientAddress + "" + Common.LoggedInClientAddress2);
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[POSTALADDRESS]", "" + Common.LoggedInClientAddress + "" + Common.LoggedInClientAddress2);
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[CELLNUMBER]", "" + Common.LoggedInClientContactNo + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[IDNUMBER]", "" + Common.LoggedInClientIDNumber + "");

                    htmlCodeToConvert = htmlCodeToConvert.Replace("[EMAILADDRESS]", "" + Common.LoggedInClientEmailAddress  + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[WORKNUMBER]", "" + " " + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[HOMENUMBER]", "" +" " + "");

                    htmlCodeToConvert = htmlCodeToConvert.Replace("[CONSUMERNAME]", "" + StaffLoans.Models.Common.LoggedInClientName + "").Replace("[DAY]", "" + DateTime.Today.Day + "").Replace("[MONTH]", "" + mfi.GetMonthName(DateTime.Today.Month) + "").Replace("[YEAR]", "" + DateTime.Today.Year + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[CURRENTDATE]", "" + DateTime.Now.ToShortDateString() + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[CONSUMERADDRESS]", "" + Common.LoggedInClientAddress + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[CONSUMERADDRESS2]", "" + Common.LoggedInClientAddress2 + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[CONSUMERCONTACTNO]", "" + Common.LoggedInClientContactNo + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[CONSUMERID]", "" + Common.LoggedInClientIDNumber + "");
                    htmlCodeToConvert = htmlCodeToConvert.Replace("[DATE]", "" + Common.GetLoggedInDate() + "");
                    //htmlCodeToConvert = htmlCodeToConvert.Replace("[CREDITAMOUNT]", "" + DateTime.Now.ToShortDateString() + "");
                    //htmlCodeToConvert = htmlCodeToConvert.Replace("[NOOFINSTALLMENTS]", "" + DateTime.Now.ToShortDateString() + "");
                    //htmlCodeToConvert = htmlCodeToConvert.Replace("[TOTALWITHINTEREST]", "" + DateTime.Now.ToShortDateString() + "");
                    //htmlCodeToConvert = htmlCodeToConvert.Replace("[INSTALLMENTAMOUNT]", "" + DateTime.Now.ToShortDateString() + "");
                    mod.salaryAndExpenses.documentInnerHTML = htmlCodeToConvert.Replace("â€“", "-").Replace("â€™", "'");
                }
                //***************************view contract************************************

                //*********************binding grid**********************************************
                Loans loans = new Models.Loans();
            }
            catch(Exception ex)
            {
                errorLogger.Info("MyAccountController=>Index" + ex.Message + " " + ex.StackTrace);
            }
            //*********************binding grid**********************************************
            return View(mod);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEmployemntDetails(StaffLoans.Models.MyAccount modal, string sendOTP, string saveData)
        {
            LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
            LoanAPI.TAffordabilityReply afforReply = new LoanAPI.TAffordabilityReply();
            LoanAPI.TInsUpdReply insUpReply = new LoanAPI.TInsUpdReply();
            List<string> data = new List<string>();
            try
            {
                Session["EmploymentDetails"] = modal.employmentDetails;
                //************************insert employer details *******************************
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    LoanAPI.TEmployerData empData = new LoanAPI.TEmployerData();
                    empData.AppointedOn = Convert.ToUInt32(modal.employmentDetails.AppointmentDate.Replace("/", ""));
                    empData.ContractEndDate = Convert.ToUInt32(modal.employmentDetails.ContractEndDate.Replace("/","")) ;
                    empData.Department = modal.employmentDetails.Department ;
                    empData.EmployeeNumber = modal.employmentDetails.EmployeeNumber ;
                    empData.Employer = modal.employmentDetails.EmployerName ;
                    empData.EmployerClientCode = Convert.ToUInt32(Common.LoggedInClientCode);
                    empData.EmployerContactNumber = modal.employmentDetails.EmployerContactNumber ;
                    empData.EmployerCountry = modal.employmentDetails.Country ;
                    empData.EmployerPostalCode = Convert.ToInt32(modal.employmentDetails.PostalCode) ;
                    empData.EmployerProvince = modal.employmentDetails.Province ;
                    empData.EmployerStreet = modal.employmentDetails.Street ;
                    empData.EmployerSuburb = modal.employmentDetails.Suburb ;
                    empData.Occupation = modal.employmentDetails.Occupation ;
                    empData.OccupationType = Convert.ToInt32(modal.employmentDetails.OccupationType) ;
                    empData.PayDayShift = Convert.ToInt32(modal.employmentDetails.Paydayshift) ;
                    empData.PayFrequency = Convert.ToInt32(modal.employmentDetails.PayFrequency);
                    empData.Placement = modal.employmentDetails.Placement ;
                    empData.RepayMethod = Convert.ToInt32(modal.employmentDetails.RepaymentMethod );
                    empData.SalaryMethod = Convert.ToInt32(modal.employmentDetails.SalaryMethod);

                    if (modal.employmentDetails.EmployerClientCode == null || modal.employmentDetails.EmployerClientCode == "0")
                    {
                        wLogger.Info("InsUpdEmployerDetails request");
                        insUpReply = proxy.InsUpdEmployerDetails(Common.getClientSessionData(), LoanAPI.TInsertUpdateType.iutInsert, empData);
                        wLogger.Info("InsUpdEmployerDetails response" + JsonConvert.SerializeObject(insUpReply));
                    }
                    else
                    {
                        wLogger.Info("InsUpdEmployerDetails request");
                        insUpReply = proxy.InsUpdEmployerDetails(Common.getClientSessionData(), LoanAPI.TInsertUpdateType.iutUpdate, empData);
                        wLogger.Info("InsUpdEmployerDetails response" + JsonConvert.SerializeObject(insUpReply));
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger.Info("MyAccountController=>SaveEmployemntDetails" + ex.Message + " " + ex.StackTrace);
            }
            return Json(data = data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBankDetails(StaffLoans.Models.MyAccount modal, string sendOTP, string saveData)
        {
            LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
            LoanAPI.TAffordabilityReply afforReply = new LoanAPI.TAffordabilityReply();
            LoanAPI.TInsUpdReply insUpReply = new LoanAPI.TInsUpdReply();
            List<string> data = new List<string>();
            try
            {

                Session["BankDetails"] = modal.bankDetails;
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    LoanAPI.TBankingData  bankData = new LoanAPI.TBankingData();
                    bankData.AccountNumber = modal.bankDetails.AccountNumber ;
                    bankData.AccountType = Convert.ToInt32( modal.bankDetails.AccountType) ;
                    bankData.BankingClientCode = Convert.ToUInt32(Common.LoggedInClientCode );
                    bankData.BranchCode = "";

                    wLogger.Info("InsUpdBankingDetails request");
                    if (modal.bankDetails.BankClientCode == null  || modal.bankDetails.BankClientCode =="0")
                        insUpReply=proxy.InsUpdBankingDetails(Common.getClientSessionData(), LoanAPI.TInsertUpdateType.iutInsert, bankData );
                    else
                        insUpReply=proxy.InsUpdBankingDetails(Common.getClientSessionData(), LoanAPI.TInsertUpdateType.iutUpdate , bankData);

                    wLogger.Info("InsUpdBankingDetails response" + JsonConvert.SerializeObject(insUpReply));
                }

            }
            catch (Exception ex)
            {
                errorLogger.Info("MyAccountController=>SaveBankDetails" + ex.Message + " " + ex.StackTrace);
            }
            return Json(data = data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CheckAffordability(StaffLoans.Models.MyAccount   modal, string sendOTP, string saveData)
        {
            LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
            LoanAPI.TAffordabilityReply afforReply = new LoanAPI.TAffordabilityReply();
            List<string> data = new List<string>();
            try {
                //using (var proxy = new LoanAPI.FINBONDAPIClient())
                //{
                //    wLogger.Info("OperatorLogon request");
                //    loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                //    wLogger.Info("OperatorLogon response" + JsonConvert.SerializeObject(loginReply));
                //}
                //if (loginReply.ReplyData.ReplyCode == 0)
                //{
                //    Common.OperatorLoginSessionId = loginReply.SessionData.SessionID;
                //}

                if (Common.getClientSessionData().SessionID != "")
                {
                    using (var proxy = new LoanAPI.FINBONDAPIClient())
                    {
                        LoanAPI.TAffordabilityInfo info = new LoanAPI.TAffordabilityInfo();
                        info.Salary1 = Convert.ToUInt32(modal.salaryAndExpenses.LastMonthSalary) * 100;
                        info.Salary2 = Convert.ToUInt32(modal.salaryAndExpenses.LastTwoMonthSalary) * 100;
                        info.Salary3 = Convert.ToUInt32(modal.salaryAndExpenses.LastThreeMonthSalary) * 100; ;

                        //int dayOfMonth = DateTime.Now.Day;
                        //var targetDate = DateTime.Now;
                        //if (dayOfMonth > 15)
                        //{
                        //    DateTime calDate = DateTime.Now.AddMonths(-1);
                        //    targetDate = new DateTime(calDate.Year, calDate.Month, 20);
                        //}
                        //else
                        //{
                        //    DateTime calDate = DateTime.Now;
                        //    targetDate = new DateTime(calDate.Year, calDate.Month, 20);
                        //}
                        info.DebitOrderRunDate = Convert.ToInt32(modal.salaryAndExpenses.DebitOrderDate);

                        info.ReasonForLoan = Convert.ToInt32(modal.salaryAndExpenses.ReasonForLoan);

                        info.Groceries = Convert.ToUInt32(modal.salaryAndExpenses.Groceries) * 100;
                        info.Transport = Convert.ToUInt32(modal.salaryAndExpenses.Transport) * 100;
                        info.Housing = Convert.ToUInt32(modal.salaryAndExpenses.Housing) * 100;
                        info.Clothing = Convert.ToUInt32(modal.salaryAndExpenses.Clothing) * 100;
                        info.Medical = Convert.ToUInt32(modal.salaryAndExpenses.Medical) * 100;
                        info.WaterElectricity = Convert.ToUInt32(modal.salaryAndExpenses.WaterElectricty) * 100;
                        info.ChildMaintenance = Convert.ToUInt32(modal.salaryAndExpenses.ChildMaintainance) * 100;
                        info.Education = Convert.ToUInt32(modal.salaryAndExpenses.Education) * 100;
                        info.BankCharges = Convert.ToUInt32(modal.salaryAndExpenses.BankCharges) * 100;
                        info.Other = Convert.ToUInt32(modal.salaryAndExpenses.Other) * 100;

                        var clientType = Convert.ToUInt32(ConfigurationManager.AppSettings["ClientType"].ToString());
                        info.CardClientType = clientType;
                        info.ClientCode = StaffLoans.Models.Common.LoggedInClientCode;
                        info.ClientIDNumber = StaffLoans.Models.Common.LoggedInClientIDNumber;
                        info.ClientType = clientType;
                        info.ClientUSN = Convert.ToUInt32(StaffLoans.Models.Common.LoggedInClientUSN);
                        info.GrantRecipient = false;
                        info.OperatorIDNumber = ConfigurationManager.AppSettings["OperatorLoginIDnumber"].ToString();
                        info.OperatorUSN = Convert.ToUInt32(ConfigurationManager.AppSettings["OperatorUSN"].ToString());

                        //  var a = proxy.CheckAffordability
                        wLogger.Info("CheckAffordability request");
                        afforReply = proxy.CheckAffordability(Common.getClientSessionData(), info);
                        wLogger.Info("CheckAffordability response" + JsonConvert.SerializeObject(afforReply));
                        //***************************bind grid with loan options
                        if (afforReply != null)
                        {
                            if (afforReply.ReplyData.ReplyCode == 0 || afforReply.ReplyData.ReplyCode == 2) //success
                            {
                                Session["LoanOptions"] = afforReply.LoanOptions;
                                Common.LoanApplicationNumber = afforReply.ApplicationNumber.ToString();
                                data = new List<string>() { "Success", "Received Loan options" };
                            }
                            else
                            {
                                data = new List<string>() { "Error", afforReply.ReplyData.ReplyCode + " " + afforReply.ReplyData.ReplyMessage };
                            }

                        }
                        else
                        {
                            data = new List<string>() { "Error", "No Loan options" };
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                errorLogger.Info("MyAccountController=>CheckAffordability" + ex.Message + " " + ex.StackTrace);
            }
            return Json(data = data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTransactions( FormCollection fc)
        {

            int iTotalRecordsinDB = 0;

            List<Loans> pRes = new List<Loans>();
            LoanAPI.TLoanOption [] lstLoans = null; ;
            try
            {
                //Loans loan = new Loans();
                //loan.Terms = "5";
                //loan.Description = "Moneyline";
                //loan.Capital = "500";
                //loan.InitiationFee = "100";
                //loan.ServiceFee = "90";
                //loan.PrincipalDebt = "7";
                //loan.FirstPaymentDate = "20191111";
                //loan.LastPaymentDate = "20191111";
                //loan.InstallationAmount = "100";
                //pRes.Add(loan);
                //pRes = ;
                //Session["PayReceived"] = pRes;
                if(Session["LoanOptions"]!=null)
                {

                    //lstLoans = (List<LoanAPI.TLoanOption>)Session["LoanOptions"];
                    lstLoans = (LoanAPI.TLoanOption[])Session["LoanOptions"];

                }
                    if (lstLoans != null && lstLoans.Count() > 0)
                    iTotalRecordsinDB = lstLoans.Count();
            }
            catch (Exception e)
            {
                errorLogger.Info("MyAccountController=>GetTransactions" + e.Message + " " + e.StackTrace);
                // CommonMaster.LogSystemError(e);
            }
         
             //var result = from t in pRes select new[] { "<div class=\"clsHightlightGridRow  clsGridRowOrange\">" + t.PaymentDate.ToString("dd MMM yyyy") + " </div>", "<a href=" + t.LinkURL + " target=\"_blank\">" + ((t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")) == "..." ? "Master Payment Link" : ((t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")).Contains("?")) ? (t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")).Substring(0, t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...").IndexOf('?')) : (t.LinkURL.Replace(CommonMaster.DefaultPayLink, "..."))) + "</a>", t.PaymentType, t.Description, t.ClientName, t.Reference, CommonMaster.FormatMoney((decimal)(t.AmountPaid / (decimal)100.00)).ToString(), CommonMaster.FormatMoney((decimal)(t.Fee / (decimal)100.00)).ToString(), "<div class=\"ExportButton\" id=\"" + t.RowNumber + "\" ><img src=\"/Content/Images/Export_icon.png\" alt=\"\" style=\"vertical-align:middle;margin-top:5px\" class=\" download \"  ></div>" };
            //var result = from t in pRes select new[] { "(<div class=\"clsHightlightGridRow  clsGridRowRed\">" + t.PaymentDate.ToString("dd MMM yyyy") + " </div>", "<a href=" + t.LinkURL + " target=\"_blank\">" + ((t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")) == "..." ? "Master Payment Link" : ((t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")).Contains("?")) ? (t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")).Substring(0, t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...").IndexOf('?')) : (t.LinkURL.Replace(CommonMaster.DefaultPayLink, "..."))) + "</a>", t.Description, t.ClientName, t.Reference, CommonMaster.FormatMoney(t.AmountPaid / 100.00).ToString(), CommonMaster.FormatMoney(t.Fee / 100.00).ToString(), "<div class=\"ExportButton\" id=\"" + t.RowNumber + "\" ><img src=\"/Content/Images/Export_icon.png\" alt=\"\" style=\"vertical-align:middle;margin-top:5px\" class=\" download \"  ></div>" };
            //    var result = from t in pRes select new[] {t.PaymentDate.ToString("yyyy-MM-dd"), "<a href=" + t.LinkURL + " target=\"_blank\">" + t.LinkURL + "</a>", t.Description, t.ClientName, t.Reference, CommonMaster.FormatMoney(t.AmountPaid).ToString(), CommonMaster.FormatMoney(t.Fee).ToString(), "<div class=\"ExportButton\" id=\"" + t.RowNumber + "\" ><img src=\"/Content/Images/Export_icon.png\" alt=\"\" style=\"vertical-align:middle;margin-top:10px\" class=\" download \"  ></div>" };
            var result = from t in lstLoans
                         select new[] { "<div><input type=\"radio\" id=\"rdoption\" GroupName=\"LoanOptions\" onclick=\"fnradiocheck(this,'" + t.LoanID + "','" + t.OfferID + "','" + t.LoanTerm + "','" + t.LoanDescription + "','" + t.Loan_Capital + "','" + t.Loan_InitiationFee + "','"+ t.Loan_ServiceFee + "','" + t.Loan_PrincipalDebt + "','"+ t.DateOfFirstDebit + "','"+ t.DateOfLastDebit + "','"+ t.Loan_InstallmentAmount + "','" + t.Loan_CapitalPlusInterest + "','" + t.Loan_TotalRepayment + "','" + t.Loan_InterestRate + "','" + t.Loan_VAT + "','" + t.Loan_InterestAmount + "')\" /></div>", t.LoanTerm.ToString(), t.LoanDescription.ToString(),Common.FormatMoney(( Convert.ToInt64( t.Loan_Capital.ToString())/100)).ToString(),Common.FormatMoney( (Convert.ToInt64( t.Loan_InitiationFee.ToString())/100)).ToString(),Common.FormatMoney( (Convert.ToInt64( t.Loan_ServiceFee.ToString())/100)).ToString(),Common.FormatMoney( (Convert.ToInt64( t.Loan_PrincipalDebt.ToString())/100)).ToString(),Common.FormatDate( t.DateOfFirstDebit.ToString()),Common.FormatDate( t.DateOfLastDebit.ToString()),Common.FormatMoney( (Convert.ToInt64( t.Loan_InstallmentAmount.ToString())/100)).ToString() };

       
            return Json(new
            {
                //sEcho = param.sEcho,
                //iTotalRecords = allTransactions.Count(),
                //iTotalDisplayRecords = filteredTransactions.Count(),
                //iTotalRecords = 100,
                iTotalRecords = iTotalRecordsinDB,
                iTotalDisplayRecords = iTotalRecordsinDB,
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);


        }


        #region Asynchronous Methods
        [HttpPost]
        //[Filters.ValidateJsonAntiForgeryTokenAttribute]
        public JsonResult UploadDocument()
        {
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["HelpSectionImages"];

                    int documentTypeID = int.Parse(System.Web.HttpContext.Current.Request.Form["UploadedDocumentTypeID"]);
                    
                    int ind = -1;
                    
                    bool isValid = false;
                    byte[] fileBytes = null;
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(pic);
                    string strExtension = StaffLoans.Models.Common.GetExtension(filebase.FileName.ToString());
                    string strContentType = filebase.ContentType;
                    if (strExtension.ToUpper() == "PDF" || StaffLoans.Models.Common.CheckValidFileFormat(strExtension.ToUpper()))
                    {
                        using (BinaryReader b = new BinaryReader(filebase.InputStream))
                        {
                            if (filebase.ContentLength < 5000000)
                            {
                                fileBytes = b.ReadBytes(filebase.ContentLength);
                                Document doc = new Document()
                                {
                                    DocName = filebase.FileName,
                                    DocExt = strExtension,
                                    DocData = fileBytes,
                                    DocTypeID = documentTypeID
                                };

                                if (Common.UploadedDocumentList.Exists(x => x.DocTypeID == documentTypeID))
                                    Common.UploadedDocumentList[Common.UploadedDocumentList.FindIndex(x => x.DocTypeID == documentTypeID)] = doc;
                                else
                                    Common.UploadedDocumentList.Add(doc);

                                //switch (documentTypeID)
                                //{
                                //    case ((int)StaffLoans.Models.Common.DocumentType.IDDocument ):
                                //        StaffLoans.Models.Common.UploadedIDDocumentName = filebase.FileName;
                                //        StaffLoans.Models.Common.UploadedIDFileExtension = strExtension;
                                //        StaffLoans.Models.Common.UploadedIDDocument = fileBytes;
                                //        break;
                                //    case ((int)StaffLoans.Models.Common.DocumentType.Payslip):
                                //        StaffLoans.Models.Common.UploadedPayslipDocumentName = filebase.FileName;
                                //        StaffLoans.Models.Common.UploadedPaySlipFileExtension = strExtension;
                                //        StaffLoans.Models.Common.UploadedPayslipDocument = fileBytes;
                                //        break;
                                //    case ((int)StaffLoans.Models.Common.DocumentType.BankStatement ):
                                //        StaffLoans.Models.Common.UploadedBankStatementDocumentName = filebase.FileName;
                                //        StaffLoans.Models.Common.UploadedBankStatementExtension = strExtension;
                                //        StaffLoans.Models.Common.UploadedBankStatementDocument = fileBytes;
                                //        break;
                                //    case ((int)StaffLoans.Models.Common.DocumentType.Selfie):
                                //        StaffLoans.Models.Common.UploadedSelfieDocumentName = filebase.FileName;
                                //        StaffLoans.Models.Common.UploadedSelfieExtension = strExtension;
                                //        StaffLoans.Models.Common.UploadedSelfieDocument = fileBytes;
                                //        break;
                                //}
                            }
                            else
                            {
                                return Json("Invalid file size");// Json(PaycharlieSystemMessages.M44);
                            }

                            isValid = StaffLoans.Models.Common.ValidateUploadedgDoc(filebase.FileName, fileBytes, strContentType);
                        }


                        if (isValid)
                        {
                            return Json(filebase.FileName);
                        }
                        else
                        {
                            if (Common.UploadedDocumentList.Exists(x => x.DocTypeID == documentTypeID))
                            {
                                var docToRemove = Common.UploadedDocumentList.First(x => x.DocTypeID == documentTypeID);
                                Common.UploadedDocumentList.Remove(docToRemove);
                            }

                            //switch (documentTypeID)
                            //{
                            //    case ((int)StaffLoans.Models.Common.DocumentType.IDDocument):
                            //        StaffLoans.Models.Common.UploadedIDDocumentName = "";
                            //        StaffLoans.Models.Common.UploadedIDFileExtension = "";
                            //        StaffLoans.Models.Common.UploadedIDDocument = null;
                            //        break;
                            //    case ((int)StaffLoans.Models.Common.DocumentType.Payslip):
                            //        StaffLoans.Models.Common.UploadedPayslipDocumentName = "";
                            //        StaffLoans.Models.Common.UploadedPaySlipFileExtension = "";
                            //        StaffLoans.Models.Common.UploadedPayslipDocument = null;
                            //        break;
                            //    case ((int)StaffLoans.Models.Common.DocumentType.BankStatement):
                            //        StaffLoans.Models.Common.UploadedBankStatementDocumentName = "";
                            //        StaffLoans.Models.Common.UploadedBankStatementExtension = "";
                            //        StaffLoans.Models.Common.UploadedBankStatementDocument = null;
                            //        break;
                            //    case ((int)StaffLoans.Models.Common.DocumentType.Selfie ):
                            //        StaffLoans.Models.Common.UploadedSelfieDocumentName = "";
                            //        StaffLoans.Models.Common.UploadedSelfieExtension = "";
                            //        StaffLoans.Models.Common.UploadedSelfieDocument = null;
                            //        break;

                            //}

                            return Json("File Upload failed");// 
                        }
                    }
                    else { return Json("Invalid File Format"); } 
                }
                else { return Json("File Upload Failed"); } 
            }
            catch (Exception ex)
            {
                errorLogger.Info("MyAccountController=>UploadDocument" + ex.Message + " " + ex.StackTrace);
                //CommonMaster.LogSystemError(ex);
                return Json("File Upload failed"); 
            }
        }
        #endregion

        #region FinalLoanSubmission
        private void FinalLoanSubmission()
        {
            
        }
        #endregion

        #region AdjustOffer
        [HttpPost]
        public JsonResult performAdjustOffer(int offerId, int newAmount)
        {
            LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
            LoanAPI.TLoanOption loanOption = new LoanAPI.TLoanOption();
            try
            {
                LoanAPI.TAdjustOfferReply offerReply = new LoanAPI.TAdjustOfferReply();
                //using (var proxy = new LoanAPI.FINBONDAPIClient())
                //{
                //    loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                //}
                //if (loginReply.ReplyData.ReplyCode == 0)
                //{
                //    Common.OperatorLoginSessionId = loginReply.SessionData.SessionID;
                //}

                if (Common.getClientSessionData().SessionID != "")
                {
                    using (var proxy = new LoanAPI.FINBONDAPIClient())
                    {
                        //loginReply = proxy.InsDocuments(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], StaffLoans.Models.Common.LoggedInClientCode,StaffLoans.Models.Common.LoggedInClientIDNumber ,StaffLoans.Models.Common.LoanApplicationNumber , );
                        wLogger.Info("AdjustOffer request");
                        offerReply = proxy.AdjustOffer(Common.getClientSessionData(), ConfigurationManager.AppSettings["OperatorLoginIDnumber"], Convert.ToUInt32(Common.LoggedInClientCode), Convert.ToUInt32(offerId), Convert.ToUInt32(newAmount) * 100);
                        wLogger.Info("AdjustOffer response" + JsonConvert.SerializeObject(offerReply));
                        if (offerReply.ReplyData.ReplyCode == 0)
                        {
                            loanOption = offerReply.NewLoanOptionOffer;

                        }
                        //proxy.InsUpdClientActivation 
                    }
                }
            }
            catch(Exception ex)
            {
                errorLogger.Info("MyAccountController=>performAdjustOffer " + ex.Message + " " + ex.StackTrace);
            }
            return Json(loanOption);
        }
        #endregion

        [HttpPost]
        public JsonResult CompleteLoanProcess(string strHTM,int iOfferID)
        {
            try
            {
                //*********************
                //**************chkec user selected loan offfer option ******************
                if (iOfferID == 0)
                {
                    return Json("Error", "Please select the loan option");
                }
                //****************chk user uploaded documents*******************
                //**********************chk user signed the contract**************************
                //if (StaffLoans.Models.Common.UploadedIDDocument == null)
                //{
                //    //ShowMessages("Please upload ID Document");
                //    return Json("Error", "Please upload ID Document");
                //}
                //if (StaffLoans.Models.Common.UploadedPayslipDocument == null)
                //{
                //    //ShowMessages("Please upload payslip Document");
                //    return Json("Error", "Please upload payslip Document");
                //}
                //if (StaffLoans.Models.Common.UploadedBankStatementDocument == null)
                //{
                //    //ShowMessages("Please upload bank statement Document");
                //    return Json("Error", "Please upload bank statement Document");
                //}
                //if (StaffLoans.Models.Common.UploadedSelfieDocument== null)
                //{
                //    //ShowMessages("Please upload bank statement Document");
                //    return Json("Error", "Please upload Selfie Document");
                //}
                if (Common.UploadedDocumentList.Count() < ExpectedNumberOfDocumentUploads)
                {
                    return Json("Error", "Please upload All Required Documents");
                }


                //*******************inserting documents.
                LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
                List<LoanAPI.TDocument> lstDoucments = new List<LoanAPI.TDocument>();

                //LoanAPI.TDocument lstIDDoc = new LoanAPI.TDocument();
                //lstIDDoc.Document = Common.UploadedIDDocument;
                //lstIDDoc.ImageType = 1;
                //lstIDDoc.MimeType = GetMimeType(Common.UploadedIDFileExtension);

                //LoanAPI.TDocument lstPayslipDoc = new LoanAPI.TDocument();
                //lstPayslipDoc.Document = Common.UploadedPayslipDocument;
                //lstPayslipDoc.ImageType = 2;
                //lstPayslipDoc.MimeType = GetMimeType(Common.UploadedPaySlipFileExtension);

                //LoanAPI.TDocument lstBankStatementDoc = new LoanAPI.TDocument();
                //lstBankStatementDoc.Document = Common.UploadedBankStatementDocument;
                //lstBankStatementDoc.ImageType = 3;
                //lstBankStatementDoc.MimeType = GetMimeType(Common.UploadedBankStatementExtension);

                //LoanAPI.TDocument lstSelfieDoc = new LoanAPI.TDocument();
                //lstSelfieDoc.Document = Common.UploadedSelfieDocument;
                //lstSelfieDoc.ImageType = 8;
                //lstSelfieDoc.MimeType = GetMimeType(Common.UploadedSelfieExtension);

                foreach (Document doc in Common.UploadedDocumentList)
                {
                    lstDoucments.Add(new LoanAPI.TDocument()
                    {
                        Document = doc.DocData,
                        ImageType = doc.DocTypeID,
                        MimeType = GetMimeType(doc.DocExt)
                    });
                }


                byte[] pdfFromHtml = GeneratePDFFrmJ(strHTM); ;
                LoanAPI.TDocument lstViewContract = new LoanAPI.TDocument();
                lstViewContract.Document = pdfFromHtml;
                lstViewContract.ImageType = 4;
                lstViewContract.MimeType = "application/pdf";

                //lstDoucments.Add(lstIDDoc);
                //lstDoucments.Add(lstPayslipDoc);
                //lstDoucments.Add(lstBankStatementDoc);
                lstDoucments.Add(lstViewContract);

                //lstDoucments.Add(lstSelfieDoc);

                List<string> data = new List<string>();
                LoanAPI.TReplyData docReply = new LoanAPI.TReplyData();

                LoanAPI.TReplyData loanComplettionReply = new LoanAPI.TReplyData();
                //using (var proxy = new LoanAPI.FINBONDAPIClient())
                //{
                //    wLogger.Info("OperatorLogon request");
                //    loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                //    wLogger.Info("OperatorLogon response" + JsonConvert.SerializeObject( loginReply));
                //}
                //if (loginReply.ReplyData.ReplyCode == 0)
                //{
                //    Common.OperatorLoginSessionId = loginReply.SessionData.SessionID;
                //}

                if (Common.ClientLoginSessionId != "")
                {
                    using (var proxy = new LoanAPI.FINBONDAPIClient())
                    {
                        docReply = proxy.InsDocuments(Common.getClientSessionData(), Convert.ToUInt32(StaffLoans.Models.Common.LoggedInClientCode), StaffLoans.Models.Common.LoggedInClientIDNumber, Convert.ToUInt32(StaffLoans.Models.Common.LoanApplicationNumber), lstDoucments.ToArray());
                        if (docReply.ReplyCode == 0)
                        {
                            //*****************loan application completion****************
                            wLogger.Info("LoanApplicationCompletion request");
                            loanComplettionReply = proxy.LoanApplicationCompletion(Common.getClientSessionData(), ConfigurationManager.AppSettings["OperatorLoginIDnumber"], Convert.ToUInt32(Common.LoggedInClientCode), Convert.ToUInt32(Common.LoggedInClientUSN), Convert.ToUInt32(iOfferID), Convert.ToUInt32(100),
                                Convert.ToUInt32(StaffLoans.Models.Common.LoanApplicationNumber), "", "", "", "");
                            wLogger.Info("LoanApplicationCompletion response" + JsonConvert.SerializeObject(loanComplettionReply));
                            if (loanComplettionReply.ReplyCode == 0)
                            {
                                TempData["ApplicationNumber"] = Common.LoanApplicationNumber;
                                //**********************send email to hr informaing about new application****************************************
                                SendEmailToHR(ConfigurationManager.AppSettings["HRMailId"].ToString(), "HR");
                                return Json("Success");
                            }
                            else
                            {
                                return Json(loanComplettionReply.ReplyMessage);
                            }

                        }
                        else
                        {
                            return Json(docReply.ReplyMessage);
                        }
                    }
                }
                else
                {
                    return Json("Invalid Session");
                }

            }
            catch(Exception ex)
            {
                errorLogger.Info("MyAccountController=>CompleteLoanProcess" + ex.Message + " " + ex.StackTrace);
            }
            return Json("Failed");
            //return Json("Success");
        }

        public void SendEmailToHR( string strEmailId, string strUserName)
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append("<map name='mapFooLink'>");
            sb.Append("	<area shape='rect' coords='133,21,283,62' href='http://www.net1.com/' />");
            //sb.Append("<area shape='rect' coords='429,29,528,48' href='http://www.net1.com/' />");
            sb.Append("</map>");
            sb.Append("<table width=\"960px\" cellpadding=\"0\" cellspacing=\"0\">");
            sb.Append("<tr>");
            sb.Append("<td ><img  usemap=\"#mapFooLink\" src=\"cid:imgHEADER\" width=\"960\" height=\"80\" alt=\"NET1\" /></td>");
            sb.Append("</tr>");
            //sb.Append("<tr><td style=\"background-color:#a7c1e4;height:4px\"></td></tr>");
            sb.Append("<tr><td style =\"font-family:Futura Lt BT;color:#4a4a4a\">");
            // sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append("Hi " + strUserName);
            sb.Append("<br/>");
            sb.Append("There is new loan request from NET1 Staffloans, Please click on below link to Approve/Reject the application");
            sb.Append("<Br/>");
            string strURL = ConfigurationManager.AppSettings["AdminPortalURL"].ToString() + "";

            sb.Append(strURL);
            sb.Append("<Br/>");
            sb.Append("<Br/>");
            sb.Append("Thank You");
            sb.Append("<Br/>");
            sb.Append("<Br/>");
            sb.Append("Regards,<Br/>");
            sb.Append("NET1 Staff Loans Team");
            sb.Append("<br />");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td ><img  src=\"cid:imgFOOTER\" width=\"960\" height=\"124\" alt=\"NET1\" /></td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            List<string> lst = new List<string>();
            lst.Add(strEmailId);
            Emails.SendEmail(sb, "Staff Loan Request", lst);
            wLogger.Info("Staff Loan Requests:  " + lst + sb.ToString());
        }

        public string GetMimeType(string strExtension)
        {
            if (strExtension.ToLower() == "pdf")
                return "application/pdf";
            else if (strExtension.ToLower() == "png")
                return "image/png";
            else if (strExtension.ToLower() == "jpg")
                return "image/jpeg";
            else if (strExtension.ToLower() == "gif")
                return "image/gif";
            else if (strExtension.ToLower() == "tif")
                return "image/tiff";
            else if (strExtension.ToLower() == "bmp")
                return "image/bmp";
            else
                return "";
        }

        public byte[] GeneratePDFFrmJ(string strHTM)
        {
            string strFileName = "";
            byte[] pdfBytes=null ;
            try
            {
                strHTM = HttpUtility.UrlDecode(strHTM);
                // Create the PDF converter. Optionally the HTML viewer width can be specified as parameter
                // The default HTML viewer width is 1024 pixels.
                PdfConverter pdfConverter = new PdfConverter();

                // set the license key - required
                pdfConverter.LicenseKey = "g6ixo7Cwo7Kxo7CttqOwsq2ysa26urq6";

                // set the converter options - optional
                pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
                pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
                pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;

                // set if header and footer are shown in the PDF - optional - default is false 
                pdfConverter.PdfDocumentOptions.ShowHeader = false;
                pdfConverter.PdfDocumentOptions.ShowFooter = false;
                // set if the HTML content is resized if necessary to fit the PDF page width - default is true
                pdfConverter.PdfDocumentOptions.FitWidth = true;

                // set the embedded fonts option - optional - default is false
                pdfConverter.PdfDocumentOptions.EmbedFonts = false;
                // set the live HTTP links option - optional - default is true
                pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;

                // set if the JavaScript is enabled during conversion to a PDF - default is true
                pdfConverter.JavaScriptEnabled = false;

                // set if the images in PDF are compressed with JPEG to reduce the PDF document size - default is true
                pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;

                //StringWriter sw = new StringWriter();
                //System.Web.HttpContext.Current.Server.Execute("~/PdfPage_1.html", sw);
                //string htmlCodeToConvert = sw.GetStringBuilder().ToString();
                string htmlCodeToConvert = strHTM;

                //string test = Request.RawUrl;
                //string url = Request.Url.Host;
                //url = Request.Url.AbsoluteUri;

                string baseURL = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/" + (System.Web.HttpContext.Current.Request.Url.AbsolutePath.Split(Convert.ToChar("/")))[1] + "/";

                //string fileNAME = "epTRANS-" + ID + "-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
                strFileName = "ComplianceDoc-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

                pdfBytes = pdfConverter.GetPdfBytesFromHtmlString(htmlCodeToConvert, baseURL);

                //var TEST = pdfBytes;
                //byte[] signedHTML = Encoding.UTF8.GetBytes(htmlCodeToConvert);


                //EmployeeDocuments empDoc = new EmployeeDocuments();
                //empDoc.EmployeeId = Common.LoggedInEmployeeID;
                //empDoc.DocumentId = iDocId;
                //empDoc.SignedDocument = pdfBytes;
                //empDoc.SignedDocumentHTML = signedHTML;
                //empDoc.stampNumber = strStampNumber;
                //string result = Common.CallApiPost("api/EmpDocuments/SaveSignedDocument/empDocument", empDoc);
                //var deserialized = JsonConvert.DeserializeObject<Boolean>(result);
                //if (deserialized)
                //{
                //    ViewBag.ShowSuccessMessage = true;
                //    ViewBag.ShowSuccessMessageDescription = "Your document uploaded successfully!";
                //}    //var TEST = ImageURL;
                //*********************save pdf document in db

                // send the PDF document as a response to the browser for download
                //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
                //response.Clear();
                //response.AddHeader("Content-Type", "application/pdf");

                //response.AddHeader("Content-Disposition", String.Format("attachment; filename=" + strFileName +
                //    "; size={0}", pdfBytes.Length.ToString()));

                //response.BinaryWrite(pdfBytes);
                //// Note: it is important to end the response, otherwise the ASP.NET
                //// web page will render its content to PDF document stream
                //response.End();

            }
            catch (Exception ex)
            {
                errorLogger.Info("MyAccountController=>GeneratePDFFrmJ " + ex.Message + " " + ex.StackTrace);
                strFileName = "";
                //EPface.insertERROR(-1, "Failed Creating PDF - " + ex.StackTrace + " - " + ex.Message);
                //Log.Insert("Common==>ConvertURLtoPDF", ex.Message, ex.StackTrace);
               // Models.Common.LogSystemError(ex);

            }
            //            return Json("Failed");
            return pdfBytes;
        }

    }
}