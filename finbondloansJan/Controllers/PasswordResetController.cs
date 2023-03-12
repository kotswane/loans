using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaffLoans.Models;
using log4net;
using Newtonsoft.Json;
using System.Text;
using System.Web.Configuration;
using System.Globalization;
using StaffLoans.LoanAPI;
using System.Configuration;

namespace StaffLoans.Controllers
{
    public class PasswordResetController : BaseController
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");

        public ActionResult Index()
        {
            
            ResetPassword model = new Models.ResetPassword();
            bool validClient = false;
            if (HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get(0) != null && HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("CIN") != null && HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("sm") != null)
            {
                //string smDate = Models.Common.Decrypt(HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("sm"), true);
                //string format = "yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss";
                //string format2 = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
                //DateTime requestDateTime = DateTime.ParseExact(smDate, format, CultureInfo.InvariantCulture);

                //bool linkAlive = CheckLinkAlive(requestDateTime); //is link expired
                //bool linkActive = false; //is link used

                string cid = Models.Common.Decrypt(HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get(0), true);
                string idNumber = Models.Common.Decrypt(HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("CIN"), true);

                try
                {
                    //using (var proxy = new FINBONDAPIClient())
                    //{
                    //    TLogonReply loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], "", "", 1, 2043034858);

                    //    if (loginReply.ReplyData.ReplyCode == 0)
                    //    {
                    //        TClientEnquiryReply clientEnquiry = proxy.ClientEnquiry(loginReply.SessionData, idNumber, true);
                    //        if (clientEnquiry.ReplyData.ReplyCode == 1)
                    //        {
                    //            if (clientEnquiry != null && !string.IsNullOrEmpty(clientEnquiry.ClientData.UpdateDateTime))
                    //            {
                    //                DateTime lastResetDateTime = DateTime.ParseExact(clientEnquiry.ClientData.UpdateDateTime, format2, CultureInfo.InvariantCulture);
                    //                if (requestDateTime > lastResetDateTime)
                    //                {
                    //                    linkActive = true;
                    //                    //clientEnquiry.ClientData.UpdateDateTime = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                    //                }
                    //            }
                    //            else
                    //            {
                    //                Console.WriteLine("Failed");
                    //            }
                    //        }
                    //    }

                    //}
                }
                catch (Exception e)
                {
                    ShowMessages("Invalid Password Reset URL");
                    return RedirectToAction("Index", "Login", new { strMsg = "Your reset link failed. Please try again, or contact us on 267 364 7700, or info@loans.com" });
                }

                if (cid != "")
                {
                    //if (linkAlive && linkActive)
                    //{
                        using (var proxy = new LoanAPI.FINBONDAPIClient())
                        {

                            var cEnquiry = proxy.ClientEnquiry(Common.getOperatorSessionData(), idNumber, true);
                            if (cEnquiry != null)
                            {
                                if (cEnquiry.ReplyData.ReplyCode == 1)
                                {
                                    if (Convert.ToUInt32(cid) == cEnquiry.ClientData.ClientCode)
                                    {
                                       // model.activeReset = smDate;
                                        model.clientCode = cid;
                                        validClient = true;
                                        return View(model);
                                    }
                                }
                            }
                        }
                    //}
                    //else
                    //{
                    //    ShowMessages("Your link has expired. Please try again, or contact us on 267 364 7700, or info@loans.com");
                    //}
                }
            }
            if (!validClient)
                ShowMessages("Invalid Password Reset URL");

            return RedirectToAction("Index", "Login", new { strMsg = "Your reset link failed. Please try again, or contact us on 267 364 7700, or info@loans.com" });
            //return View(model);
        }

        private static bool CheckLinkAlive(DateTime created)
        {
            DateTime currentTime = DateTime.Now;

            try
            {

                int aliveDurationMinutes = Int32.Parse(WebConfigurationManager.AppSettings["ResetTimeMinutes"]);

                TimeSpan elapsedTime = currentTime - created;

                return (elapsedTime.TotalMinutes >= 0 && elapsedTime.TotalMinutes < aliveDurationMinutes);

                //return (elapsedTime.TotalSeconds > -60 && elapsedTime.TotalSeconds < 0) // up to 1 second before
                //       || (elapsedTime.TotalSeconds >= 0 && Math.Floor(elapsedTime.TotalSeconds) <= aliveDurationMinutes * 60); // up to 15 minutes later
            }
            catch (Exception e)
            {
                //Console.WriteLine("err: " + e);
            }

            return false;
        }
        [ValidateAntiForgeryToken]
        public ActionResult Reset(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    //string format = "yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss";
                    //DateTime requestDateTime = DateTime.ParseExact(model.activeReset, format, CultureInfo.InvariantCulture);
                    //bool linkAlive = CheckLinkAlive(requestDateTime); //is link expired

                    LoanAPI.TReplyData activationReply = new LoanAPI.TReplyData();

                    //if (linkAlive)
                    //{
                        wLogger.Info("InsUpdClientActivation request");
                        activationReply = proxy.InsUpdClientActivation(Common.getOperatorSessionData(), Convert.ToUInt32(model.clientCode), Common.Encrypt(model.Password, true), "");
                        wLogger.Info("InsUpdClientActivation response" + JsonConvert.SerializeObject(activationReply));
                        if (activationReply.ReplyCode == 1)
                        {

                        wLogger.Info("Password Reset Complete");
                        FinService.FinServiceClient client = new FinService.FinServiceClient();
                        client.SendSMS(Common.LoggedInClientContactNo, "FINBOND CREDIT: You have successfully reset your password. Call 086 000 4249 for assistance", "FinLoan");
                        client.Close();
                        wLogger.Info("PASSWORD RESET CONFIRMATION SENT TO:" + Common.LoggedInClientContactNo + " AT: " + DateTime.Now);

                        return RedirectToAction("Index", "Home", new { strMessage = "SP_Password reset successfully" });
                        }
                    //}
                    //else
                    //{
                    //    return RedirectToAction("Index", "Login", new { strMsg = "Your reset link expired. Please try again, or contact us on 267 364 7700, or info@loans.com" });
                    //}
                }
            }
            return RedirectToAction("Index", "Login", new { strMsg = "Your reset link failed. Please try again, or contact us on 267 364 7700, or info@loans.com" });
        }
    }
}