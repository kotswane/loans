using StaffLoans.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using log4net;

namespace StaffLoans.Controllers
{
    public class LoanOverviewController : BaseController
    {
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");

        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");

        public static string FormatMoney(decimal dValue)
        {
            return String.Format(CultureInfo.CreateSpecificCulture("en-ZA"), "{0:C}", dValue).Replace(",", ".");
        }

        [CustomAuthorize]
        public ActionResult Index()
        {
            LoanInfo loans = new LoanInfo();
            try
            {
                System.Globalization.CultureInfo cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("en-ZA");

                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
                var test = FormatMoney(Convert.ToDecimal(200.00));

                //Common.LoggedInClientIDNumber = "9406180969180";
                if (Common.LoggedInClientIDNumber != null)
                {
                    LoanAPI.TClientEnquiryReply clientReply = new LoanAPI.TClientEnquiryReply();
                    using (var proxy = new LoanAPI.FINBONDAPIClient())
                    {
                        //var a = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                        wLogger.Info("ClientEnquiry Request");
                        clientReply = proxy.ClientEnquiry(Common.getClientSessionData(), Common.LoggedInClientIDNumber, false);
                        wLogger.Info("ClientEnquiry Response" + JsonConvert.SerializeObject(clientReply));
                        if (clientReply.ExistingLoanData != null && clientReply.ExistingLoanData.Count() > 0)
                        {
                            loans.Name = clientReply.ClientData.FirstName;
                            loans.Surname = clientReply.ClientData.LastName;
                            loans.ContractDate = clientReply.ExistingLoanData[0].LoanDate.ToString();
                            loans.LoanAmt = Convert.ToDecimal(clientReply.ExistingLoanData[0].Loan_Capital) / 100;
                            loans.LastPayDate = clientReply.ExistingLoanData[0].LastPayDate.ToString();
                            loans.RemainingBalance = Convert.ToDecimal(clientReply.ExistingLoanData[0].TotalOutstandingAmount) / 100;
                            loans.NoOfInstallments = Convert.ToInt32(clientReply.ExistingLoanData[0].LoanTerm);
                            loans.NoOfInstallmentsPaid = Convert.ToInt32(clientReply.ExistingLoanData[0].LoanTerm) - Convert.ToInt32(clientReply.ExistingLoanData[0].InstallmentsRemaining);
                            loans.NextInstallment = Convert.ToInt32(clientReply.ExistingLoanData[0].Loan_InstallmentAmount) / 100;
                            loans.NextInstallmentDate = clientReply.ExistingLoanData[0].NextPayDate.ToString();
                            List<PaymentHistory> lst = new List<PaymentHistory>();
                            PaymentHistory p1 = new PaymentHistory();
                            p1.MonthName = "Jan";
                            p1.IsPaid = true;
                            PaymentHistory p2 = new PaymentHistory();
                            p2.MonthName = "Feb";
                            p2.IsPaid = true;
                            PaymentHistory p3 = new PaymentHistory();
                            p3.MonthName = "Mar";
                            p3.IsPaid = true;
                            PaymentHistory p4 = new PaymentHistory();
                            p4.MonthName = "Apr";
                            p4.IsPaid = false;
                            PaymentHistory p5 = new PaymentHistory();
                            p5.MonthName = "May";
                            p5.IsPaid = false;
                            PaymentHistory p6 = new PaymentHistory();
                            p6.MonthName = "June";
                            p6.IsPaid = false;
                            lst.Add(p1);
                            lst.Add(p2);
                            lst.Add(p3);
                            lst.Add(p4);
                            lst.Add(p5);
                            lst.Add(p6);



                            loans.lstPayments = lst;
                        }
                    }
                    //loans = GetLoanInformation();
                    //if (loans.LoanStatus == "Past Due")
                    //    loans.arrearsCssClass = "clsLoanInArrears";
                    //else
                    //    loans.arrearsCssClass = "noclass";
                    //if (loans.NoOfInstallments < 7)
                    //    loans.monthsPadCssClass = "clsMonthPad";
                    //else
                    //    loans.monthsPadCssClass = "";
                    //List<PaymentHistory> lst = new List<PaymentHistory>();
                    //string result = Common.CallApiGet("api/Dashboard/GetPaymentHistory/" + loans.ContractNo + "");
                    //var deserialized = JsonConvert.DeserializeObject<List<PaymentHistory>>(result);
                    //loans.lstPayments = deserialized;
                    //TempData["accountName"] = loans.EasypayRefNumber;
                    //***************************send sms based on loan status******************************
                    //SendSMSBasedOnLoanStatus(loans);
                }
                //else
                //    RedirectToAction("Index", "dashboardlogin");
            }
            catch(Exception ex)
            {
                errorLogger.Info("LoanOverviewController =>Index" + ex.Message + " " + ex.StackTrace);
            }
                ////******************send smd to the user
                return View("Index", loans);
        }
    }
}