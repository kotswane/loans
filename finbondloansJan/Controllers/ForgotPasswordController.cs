using StaffLoans.LoanAPI;
using StaffLoans.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using TwoFactor;
using log4net;

namespace StaffLoans.Controllers
{
    public class ForgotPasswordController : BaseController
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");
        private const string SESS_FORGOT_PASSWORD_OTP = "ForgotPasswordOTP";
        private const string SESS_FORGOT_PASSWORD_OTP_FAILED_ATTEMPTS = "ForgotPasswordOTPFailedAttempts";


        private static string ForgotPasswordOTP
        {
            get
            {
                if (System.Web.HttpContext.Current.Session[SESS_FORGOT_PASSWORD_OTP] != null)
                    return System.Web.HttpContext.Current.Session[SESS_FORGOT_PASSWORD_OTP].ToString();
                else
                    return "";
            }
            set
            {
                System.Web.HttpContext.Current.Session[SESS_FORGOT_PASSWORD_OTP] = value;
            }
        }

        private static int ForgotPasswordOTPFailedAttempts
        {
            get
            {
                if (System.Web.HttpContext.Current.Session[SESS_FORGOT_PASSWORD_OTP_FAILED_ATTEMPTS] != null)
                    return (int)System.Web.HttpContext.Current.Session[SESS_FORGOT_PASSWORD_OTP_FAILED_ATTEMPTS];
                else
                    return 0;
            }
            set
            {
                System.Web.HttpContext.Current.Session[SESS_FORGOT_PASSWORD_OTP_FAILED_ATTEMPTS] = value;
            }

        }


        public ActionResult Index(/*int sentStatus = 0*/)
        {
            try
            {
                //if (sentStatus == 0)
                //{
                ForgotPasswordOTP = null;
                ForgotPasswordOTPFailedAttempts = 0;

                ViewBag.ShowSuccessMessage = "False";
                ViewBag.ShowErrorMessage = "False";
                //}
                //else if (sentStatus == 1)
                //{
                //    ViewBag.ShowSuccessMessage = "True";
                //    ViewBag.ShowSuccessMessageDescription = "Reset Link Sent";
                //}
                //else
                //{
                //    ViewBag.ShowErrorMessage = "True";
                //    ViewBag.ShowerrorMessageDescription = "Failed to reset password. Please contact us on 267 364 7700, or info@loans.com";
                //}
            }
            catch (Exception ex)
            {
                errorLogger.Info("ForgotPasswordController=>Index" + ex.Message + " " + ex.StackTrace);

                return RedirectToAction("Index", "Login", new { strMsg = "An error occurred. Please try again, or contact us on 267 364 7700, or info@loans.com" });
            }

            ForgotPassword fP = new ForgotPassword();

            //if (sentStatus != 0)
            //{
            //    fP.sentStatus = sentStatus;
            //}

            return View(fP);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SendOTP(ForgotPassword fP)
        {
            try
            {
                using (var proxy = new FINBONDAPIClient())
                {
                    //TLogonReply loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"],"","", 1, 2043034858);
                    //if (loginReply.ReplyData.ReplyCode == 0)
                    //{
                    TClientEnquiryReply clientEnquiry = proxy.ClientEnquiry(Common.getOperatorSessionData(), fP.Step1.IDNumber, true);
                    if (clientEnquiry.ReplyData.ReplyCode == 1)
                    {
                        if (clientEnquiry != null)
                        {
                            ForgotPasswordOTP = Common.GenerateOTP().ToString();
                            ForgotPasswordOTPFailedAttempts = 0;

                            wLogger.Info("OTP request");
                            FinService.FinServiceClient client = new FinService.FinServiceClient();
                            client.SendSMS(clientEnquiry.ClientData.MobileNumber, "FINBOND CREDIT: You are about to reset your password. OTP is " + ForgotPasswordOTP + ". Please enter this OTP to continue. Call 086 000 4249 for assistance", "FinLoan");
                            client.Close();
                            wLogger.Info("OTP SENT TO:" + clientEnquiry.ClientData.MobileNumber + "with OTP " + ForgotPasswordOTP + " AT: " + DateTime.Now);

                            return Json("OTP sent", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Error Sending OTP", JsonRequestBehavior.AllowGet);
                        }
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                errorLogger.Info("ForgotPasswordController=>SendOTP" + ex.Message + " " + ex.StackTrace);
                return Json("Error Sending OTP", JsonRequestBehavior.AllowGet);
            }

            return Json("Error Sending OTP", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ValidateOTP(ForgotPassword fP)
        {
            if (ForgotPasswordOTP != fP.Step2.OTP)
            {
                ForgotPasswordOTPFailedAttempts++;

                if (ForgotPasswordOTPFailedAttempts >= 3)
                    return Json("Error, failed attempt limit exceeded", JsonRequestBehavior.AllowGet);

                return Json("Error, incorrect OTP", JsonRequestBehavior.AllowGet);
            }

            if (ForgotPasswordOTPFailedAttempts > 3)//just in case we are hacked.
                return Json("Error, failed attempt limit exceeded", JsonRequestBehavior.AllowGet);

            try
            {
                using (var proxy = new FINBONDAPIClient())
                {
                    TClientEnquiryReply clientEnquiry = proxy.ClientEnquiry(Common.getOperatorSessionData(), fP.Step1.IDNumber, true);
                    if (clientEnquiry.ReplyData.ReplyCode == 1)
                    {
                        if (clientEnquiry != null)
                        {
                            SendResetEmail(clientEnquiry.ClientData.EMailAddress, clientEnquiry.ClientData.FirstName, clientEnquiry.ClientData.ClientCode.ToString(), fP.Step1.IDNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger.Info("ForgotPasswordController=>ValidateOTP" + ex.Message + " " + ex.StackTrace);
                return Json("Error Sending Link via Email", JsonRequestBehavior.AllowGet);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendResetEmail(ForgotPassword fP)
        {

            try
            {
                using (var proxy = new FINBONDAPIClient())
                {
                    //TLogonReply loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"],"","", 1, 2043034858);
                    //if (loginReply.ReplyData.ReplyCode == 0)
                    //{
                    TClientEnquiryReply clientEnquiry = proxy.ClientEnquiry(Common.getOperatorSessionData(), fP.Step1.IDNumber, true);
                    if (clientEnquiry.ReplyData.ReplyCode == 1)
                    {
                        //if (clientEnquiry != null && !string.IsNullOrEmpty(clientEnquiry.ClientData.GoogleTwoFactorSecret))
                        //{
                        //    if (TimeBasedOneTimePassword.IsValid(clientEnquiry.ClientData.GoogleTwoFactorSecret, fP.TwoFactorCode))
                        //    {
                        //SendResetEmail(clientEnquiry.ClientData.EMailAddress, clientEnquiry.ClientData.FirstName, clientEnquiry.ClientData.ClientCode.ToString(), fP.IDNumber);

                        return RedirectToAction("Index", "ForgotPassword", new { sentStatus = 1 });
                        //    }
                        //    else
                        //    {
                        //        Console.WriteLine("Failed");
                        //    }
                        //}
                        //else
                        //{
                        //    Console.WriteLine("Failed");
                        //}
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                errorLogger.Info("ForgotPasswordController=>SendResetEmail" + ex.Message + " " + ex.StackTrace);
                return RedirectToAction("Index", "ForgotPassword", new { sentStatus = 2 });
            }

            return RedirectToAction("Index", "ForgotPassword", new { sentStatus = 2 });

            //return View("Index", "Login");
        }

        private void SendResetEmail(string email, string firstName, string clientCode, string idNumber)
        {
            StringBuilder sb = new StringBuilder("");
            //sb.Append("<map name='mapFooLink'>");
            //sb.Append("	<area shape='rect' coords='133,21,283,62' href='http://www.net1.com/' />");
            //sb.Append("</map>");
            sb.Append("<table width=\"960px\" cellpadding=\"0\" cellspacing=\"0\">");
            sb.Append("<tr>");
            sb.Append("<td ><img  usemap=\"#mapFooLink\" src=\"cid:imgHEADER\" width=\"960\"  height=\"216\" alt=\"Finbond\" /></td>");
            sb.Append("</tr>");
            sb.Append("<tr><td style =\"font-family:Futura Lt BT;color:#4a4a4a\">");
            sb.Append("<br />");
            sb.Append("Good day " + firstName + ", ");
            sb.Append("<br/>");
            sb.Append("<br/>");

            sb.Append("Please follow the link below to reset your password");

            sb.Append("<br/>");
            sb.Append("<Br/>");

            string submissionDate = DateTime.Now.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss", CultureInfo.InvariantCulture);
            string encrSubDate = Common.Encrypt(submissionDate, true);
            string encoSubDate = HttpUtility.UrlEncode(encrSubDate, Encoding.ASCII);

            string encryptedClientCode = Common.Encrypt(clientCode, true);
            string encryptedIDNumber = Common.Encrypt(idNumber, true);

            string encodedClientCode = HttpUtility.UrlEncode(encryptedClientCode, Encoding.ASCII);
            string encodedIDNumber = HttpUtility.UrlEncode(encryptedIDNumber, Encoding.ASCII);
            //+ "&OP2=" + encoOP2 + "&OP3=" + encoOP3 
            sb.Append(WebConfigurationManager.AppSettings["SendLink"] + encodedClientCode + "&CIN=" + encodedIDNumber + "&sm=" + encoSubDate + "");

            sb.Append("<br/>");
            sb.Append("<Br/>");
            sb.Append("Thank You,");
            sb.Append("<Br/>");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\"Regards,</div>");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">Finbond Credit Team</div>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">If you did not apply for this services please ignore this or email:loans@finbond.co.za  or contact our call center: 086 000 4249 to log a case</div>");
            sb.Append("<br />");
            sb.Append("<Br/>");
            sb.Append("<Br/>");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td ><img  src=\"cid:imgFOOTER\" width=\"960\" height=\"489\" alt=\"Finbond\" /></td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<Br/>");
            List<string> lst = new List<string>();
            lst.Add(email);
            Emails.SendEmail(sb, "Password Reset", lst);
            wLogger.Info("Forgot Password:  " + lst + sb.ToString());
        }
    }
}