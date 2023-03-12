using log4net;
using Newtonsoft.Json;
using StaffLoans.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffLoans.Controllers
{
    public class CheckEligibilityController : BaseController
    {

        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");

        public ActionResult Affordability(string strMessage)
        {
            if (!string.IsNullOrEmpty(strMessage))
                ShowMessages(strMessage);

            CheckEligibility checkElig = new CheckEligibility();
            if (TempData["checkeligilibity"]!=null )
            {
                checkElig = (CheckEligibility)TempData["checkeligilibity"];
            }
                return View("affordability", checkElig);
        }

        public ActionResult Index(string strMessage)
        {
            CheckEligibility checkElig = new CheckEligibility();

            if (!string.IsNullOrEmpty(strMessage))
                ShowMessages(strMessage);

            try

            {
                LoanAPI.TConnectionCheckReply conReply = new LoanAPI.TConnectionCheckReply();

                if (TempData["CheckEligibility"] != null)
                {
                    checkElig = (CheckEligibility)TempData["CheckEligibility"];
                }
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    wLogger.Info("ConnectionCheck request");
                    conReply = proxy.ConnectionCheck(1, 1, Convert.ToUInt32(ConfigurationManager.AppSettings["Origin"].ToString()));
                    wLogger.Info("ConnectionCheck response" + JsonConvert.SerializeObject(conReply));
                }
                if (conReply != null)
                {
                    if (conReply.LookupValues != null)
                    {
                        #region CountryOfBirth
                        checkElig.CountryOfBirthList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "COUNTRY OF BIRTH").ToList()
                                                select new CountryOfBirthItem()
                                                {
                                                    COBId = r.Code,
                                                    COBDesc = r.Description
                                                }
                                          ).ToList();
                        checkElig.CountryOfBirth = 10;
                        #endregion
                    }
                }
                if (conReply != null)
                {
                    if (conReply.LookupValues != null)
                    {
                        #region TypesOfEmployment
                        checkElig.EmploymentTypeList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "EMPLOYMENT TYPE").ToList()
                                                select new EmploymentType()
                                                {
                                                    EmploymentTypeId = r.Code,
                                                    EmploymentTypeDesc = r.Description
                                                }
                                          ).ToList();
                        //checkElig.EmploymentTypeId = 1;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger.Info("CheckEligibilityController => Index" + ex.Message + " " + ex.StackTrace);
            }

            return View(checkElig);
        }

        [ValidateAntiForgeryToken]
        public ActionResult SubmitEligibilityCheck(CheckEligibility checkData)
        {
            try
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    using (var proxy = new LoanAPI.FINBONDAPIClient())
                    {

                        wLogger.Info("LoanPreAuth request " + checkData.IDNumber + " "+  checkData.FirstName + " " + checkData.Surname + " " + Convert.ToInt32(checkData.LoanAmount) * 100 + " " + checkData.CountryOfBirth + " " + checkData.EmploymentTypeId);
                        var preAuthReply = proxy.LoanPreAuth(Common.getOperatorSessionData(), checkData.IDNumber, checkData.FirstName, checkData.Surname, Convert.ToInt32(checkData.LoanAmount) * 100, checkData.CountryOfBirth, checkData.EmploymentTypeId);
                        wLogger.Info("LoanPreAuth response" + JsonConvert.SerializeObject(preAuthReply));
                        if (preAuthReply.ReplyCode == 0)
                        {
                            //return RedirectToAction("Index", new { strMessage = "SP_Congratulations! You may qualify for a loan subject to affordability." });

                            TempData["checkeligilibity"] = checkData;
                            return RedirectToAction("Affordability", new { strMessage = "SP_Congratulations! You may qualify for a loan subject to affordability." });
                        }
                        else
                        {
                            return RedirectToAction("Index", new { strMessage = "SE_Unfortunately, you do not meet the criteria for pre-approval. Please contact us on 267 364 7700, or info@loans.com for further assistance" });
                        }
                            
                    }
                }
                return RedirectToAction("Index", "Home", new { strMsg = "An unexpected error occurred. Please try again, or contact us on 267 364 7700, or info@loans.com" });
            }
            catch (Exception ex)
            {
                errorLogger.Info("CheckEligibilityController => SubmitEligibilityCheck" + ex.Message + " " + ex.StackTrace);
                return RedirectToAction("Index", "Home", new { strMsg = "An unexpected error occurred. Please try again, or contact us on 267 364 7700, or info@loans.com" });
            }
        }

        public ActionResult GetTransactions(FormCollection fc)
        {

            int iTotalRecordsinDB = 0;

            List<Loans> pRes = new List<Loans>();
            LoanAPI.TLoanOption[] lstLoans = null; ;
            try
            {
               
                if (Session["LoanOptions"] != null)
                {

                    //lstLoans = (List<LoanAPI.TLoanOption>)Session["LoanOptions"];
                    lstLoans = (LoanAPI.TLoanOption[])Session["LoanOptions"];

                }
                if (lstLoans != null && lstLoans.Count() > 0)
                    iTotalRecordsinDB = lstLoans.Count();
            }
            catch (Exception e)
            {
                errorLogger.Info("CheckEligibilityController=>GetTransactions" + e.Message + " " + e.StackTrace);
                // CommonMaster.LogSystemError(e);
            }

            //var result = from t in pRes select new[] { "<div class=\"clsHightlightGridRow  clsGridRowOrange\">" + t.PaymentDate.ToString("dd MMM yyyy") + " </div>", "<a href=" + t.LinkURL + " target=\"_blank\">" + ((t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")) == "..." ? "Master Payment Link" : ((t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")).Contains("?")) ? (t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")).Substring(0, t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...").IndexOf('?')) : (t.LinkURL.Replace(CommonMaster.DefaultPayLink, "..."))) + "</a>", t.PaymentType, t.Description, t.ClientName, t.Reference, CommonMaster.FormatMoney((decimal)(t.AmountPaid / (decimal)100.00)).ToString(), CommonMaster.FormatMoney((decimal)(t.Fee / (decimal)100.00)).ToString(), "<div class=\"ExportButton\" id=\"" + t.RowNumber + "\" ><img src=\"/Content/Images/Export_icon.png\" alt=\"\" style=\"vertical-align:middle;margin-top:5px\" class=\" download \"  ></div>" };
            //var result = from t in pRes select new[] { "(<div class=\"clsHightlightGridRow  clsGridRowRed\">" + t.PaymentDate.ToString("dd MMM yyyy") + " </div>", "<a href=" + t.LinkURL + " target=\"_blank\">" + ((t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")) == "..." ? "Master Payment Link" : ((t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")).Contains("?")) ? (t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...")).Substring(0, t.LinkURL.Replace(CommonMaster.DefaultPayLink, "...").IndexOf('?')) : (t.LinkURL.Replace(CommonMaster.DefaultPayLink, "..."))) + "</a>", t.Description, t.ClientName, t.Reference, CommonMaster.FormatMoney(t.AmountPaid / 100.00).ToString(), CommonMaster.FormatMoney(t.Fee / 100.00).ToString(), "<div class=\"ExportButton\" id=\"" + t.RowNumber + "\" ><img src=\"/Content/Images/Export_icon.png\" alt=\"\" style=\"vertical-align:middle;margin-top:5px\" class=\" download \"  ></div>" };
            //    var result = from t in pRes select new[] {t.PaymentDate.ToString("yyyy-MM-dd"), "<a href=" + t.LinkURL + " target=\"_blank\">" + t.LinkURL + "</a>", t.Description, t.ClientName, t.Reference, CommonMaster.FormatMoney(t.AmountPaid).ToString(), CommonMaster.FormatMoney(t.Fee).ToString(), "<div class=\"ExportButton\" id=\"" + t.RowNumber + "\" ><img src=\"/Content/Images/Export_icon.png\" alt=\"\" style=\"vertical-align:middle;margin-top:10px\" class=\" download \"  ></div>" };
            var result = from t in lstLoans
                         select new[] {t.LoanTerm.ToString(), t.LoanDescription.ToString(), Common.FormatMoney((Convert.ToInt64(t.Loan_Capital.ToString()) / 100)).ToString(), Common.FormatMoney((Convert.ToInt64(t.Loan_InitiationFee.ToString()) / 100)).ToString(), Common.FormatMoney((Convert.ToInt64(t.Loan_ServiceFee.ToString()) / 100)).ToString(), Common.FormatMoney((Convert.ToInt64(t.Loan_PrincipalDebt.ToString()) / 100)).ToString(), Common.FormatDate(t.DateOfFirstDebit.ToString()), Common.FormatDate(t.DateOfLastDebit.ToString()), Common.FormatMoney((Convert.ToInt64(t.Loan_InstallmentAmount.ToString()) / 100)).ToString() };


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

        public uint getLowestSalary(uint sal1,uint sal2,uint sal3)
        {
            var numbers = new List<uint>(); // empty list 
            numbers.Add(sal1);
            numbers.Add(sal2);
            numbers.Add(sal3);
            uint minNumber = numbers.Min();
            return minNumber;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult CheckAffordability(StaffLoans.Models.CheckEligibility modal, string sendOTP, string saveData)
        {
            string CultureName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            CultureInfo ci = new CultureInfo(CultureName);
            //modal.IDNumber = "8606131285187";
            if (ci.NumberFormat.NumberDecimalSeparator != ".")
            {
                // Forcing use of decimal separator for numerical values
                ci.NumberFormat.NumberDecimalSeparator = ".";
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
            }


            LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
            LoanAPI.TAffordabilityReply afforReply = new LoanAPI.TAffordabilityReply();

            LoanAPI.TCheckBureauReply bureauReply = new LoanAPI.TCheckBureauReply();
            List<string> data = new List<string>();
            try
            {
             
                //if (Common.getClientSessionData().SessionID != "")
                //{
                    using (var proxy = new LoanAPI.FINBONDAPIClient())
                    {
                        LoanAPI.TAffordabilityInfo info = new LoanAPI.TAffordabilityInfo();
                        info.Salary1 = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.LastMonthSalary/*.Replace('.', ',')*/) * 100);
                        info.Salary2 = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.LastTwoMonthSalary/*.Replace('.', ',')*/) * 100);
                        info.Salary3 = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.LastThreeMonthSalary/*.Replace('.', ',')*/) * 100);
                        info.CustomerDeclaredIncome = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.LastMonthSalary/*.Replace('.', ',')*/) * 100);
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

                        //info.DebitOrderRunDate = Convert.ToInt32(modal.salaryAndExpenses.DebitOrderDate);

                        //info.ReasonForLoan = Convert.ToInt32(modal.salaryAndExpenses.ReasonForLoan);

                        info.Groceries = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.Groceries.ToString()/*.Replace('.', ',')*/) * 100);
                        info.Transport = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.Transport.ToString()/*.Replace('.', ',')*/) * 100);
                        info.Housing = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.Housing.ToString()/*.Replace('.', ',')*/) * 100);
                        info.Clothing = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.Clothing.ToString()/*.Replace('.', ',')*/) * 100);
                        info.Medical = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.Medical.ToString()/*.Replace('.', ',')*/) * 100);
                        info.WaterElectricity = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.WaterElectricty.ToString()/*.Replace('.', ',')*/) * 100);
                        info.ChildMaintenance = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.ChildMaintainance.ToString()/*.Replace('.', ',')*/) * 100);
                        info.Education = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.Education.ToString()/*.Replace('.', ',')*/) * 100);
                        info.BankCharges = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.BankCharges.ToString()/*.Replace('.', ',')*/) * 100);
                        info.Other = Convert.ToUInt32(Convert.ToDecimal(modal.salaryAndExpenses.Other.ToString()/*.Replace('.', ',')*/) * 100);

                        var clientType = Convert.ToUInt32(ConfigurationManager.AppSettings["ClientType"].ToString());
                        info.CardClientType = clientType;
                        info.ClientCode = 0;
                        info.ClientIDNumber = modal.IDNumber;
                        info.ClientType = clientType;
                        info.ClientUSN = 0;
                        info.GrantRecipient = false;
                        info.OperatorIDNumber = ConfigurationManager.AppSettings["OperatorLoginIDnumber"].ToString();
                        info.OperatorUSN = Convert.ToUInt32(ConfigurationManager.AppSettings["OperatorUSN"].ToString());
                        info.EmploymentType = Convert.ToUInt32(modal.EmploymentTypeId);

                        wLogger.Info("CheckBureau request " + clientType + " " +  modal.IDNumber + " " + modal.FirstName + " " + modal.Surname + " " + 0 + " " + getLowestSalary(info.Salary1,info.Salary2,info.Salary3) );
                        bureauReply = proxy.CheckBureau(Common.getOperatorSessionData(), 0, clientType, modal.IDNumber, modal.FirstName , modal.Surname, 0, getLowestSalary(info.Salary1, info.Salary2, info.Salary3));
                        wLogger.Info("CheckBureau response" + JsonConvert.SerializeObject(bureauReply));
                        if (bureauReply.ReplyData.ReplyCode==0)
                        {
                            info.CodixResult = bureauReply.CodixResult;
                            info.CodixNrMonths = bureauReply.CodixNrMonths;
                            info.BureauExpenses = bureauReply.BureauExpenses;
                        }
                    
                        //  var a = proxy.CheckAffordability
                        wLogger.Info("CheckAffordability request " + JsonConvert.SerializeObject(info));
                        afforReply = proxy.CheckAffordability(Common.getOperatorSessionData(), info);
                        wLogger.Info("CheckAffordability response" + JsonConvert.SerializeObject(afforReply));
                        //***************************bind grid with loan options
                        if (afforReply != null)
                        {
                            if (afforReply.ReplyData.ReplyCode == 0 || afforReply.ReplyData.ReplyCode == 2) //success
                            {
                                Session["LoanOptions"] = afforReply.LoanOptions;
                                //Common.LoanApplicationNumber = afforReply.ApplicationNumber.ToString();
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
               // }
            }
            catch (Exception ex)
            {
                errorLogger.Info("CheckEligibilityController=>CheckAffordability" + ex.Message + " " + ex.StackTrace);
            }
            return Json(data = data, JsonRequestBehavior.AllowGet);
        }

    }
}