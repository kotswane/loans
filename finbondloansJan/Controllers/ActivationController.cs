using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StaffLoans;
using TwoFactor;
using System.Security.Cryptography;
using System.Text;
using StaffLoans.Models;
using StaffLoans.LoanAPI;
using log4net;
using System.Configuration;
using System.Globalization;
using System.Web.Configuration;
using Newtonsoft.Json;

namespace StaffLoans.Controllers
{
    public class ActivationController : BaseController
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");
        private const string SESS_ACTIVATION_OTP = "ActivationOTP";
        private const string SESS_ACTIVATION_OTP_FAILED_ATTEMPTS = "ActivationOTPFailedAttempts";
        private static string ActivationOTP
        {
            get
            {
                if (System.Web.HttpContext.Current.Session[SESS_ACTIVATION_OTP] != null)
                    return System.Web.HttpContext.Current.Session[SESS_ACTIVATION_OTP].ToString();
                else
                    return "";
            }
            set
            {
                System.Web.HttpContext.Current.Session[SESS_ACTIVATION_OTP] = value;
            }
        }

        private static int ActivationOTPFailedAttempts
        {
            get
            {
                if (System.Web.HttpContext.Current.Session[SESS_ACTIVATION_OTP_FAILED_ATTEMPTS] != null)
                    return (int)System.Web.HttpContext.Current.Session[SESS_ACTIVATION_OTP_FAILED_ATTEMPTS];
                else
                    return 0;
            }
            set
            {
                System.Web.HttpContext.Current.Session[SESS_ACTIVATION_OTP_FAILED_ATTEMPTS] = value;
            }

        }

        public ActionResult FailedOTP()
        {
            return RedirectToAction("Index", "Login", new { strMsg = "You have entered your OTP incorrectly 3 times. Please contact us on 267 364 7700, or info@loans.com to continue setting up your account." });
        }


        public ActionResult Index(bool generateSecret = true, string error = null, string code = null, string id = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.ShowErrorMessage = "True";
                ViewBag.ShowErrorMessageDescription = error;
                //return View();
            }

            Activation model = new Activation();

            try
            {
                if (code != null && id != null)
                {
                    ActivationOTP = null;
                    ActivationOTPFailedAttempts = 0;

                    model = Activation.ValidateActivationLink(uint.Parse(code), id);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { strMsg = "SE_Failed to validate link. Please try again." });
                }


                if (model.replyData.ReplyCode != 1)
                {
                    ViewBag.ShowErrorMessage = "True";
                    ViewBag.ShowErrorMessageDescription = model.replyData.ReplyMessage;
                    return View();
                }
                TempData["activationModal"] = model;

                return View(model);
            }
            catch (Exception ex)
            {
                errorLogger.Info("ActivationCotnroller=>Index" + ex.Message + " " + ex.StackTrace);
                ViewBag.ShowErrorMessage = "True";
                ViewBag.ShowErrorMessageDescription = "An error occurred while validating your pending registration. Please try again later, or contact the call center.";
                return View();
            }
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PreserveQueryString]
        public ActionResult OTPPrompt(ActivationOTP model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ActivationOTP == model.CellNumberOTPPrompt && ActivationOTPFailedAttempts <= 3)
                    {
                        Activation.SetClientActivation(model.ClientCode, "FinbondLoan@Test", null);

                        return RedirectToAction("Index", "Login", new
                        {
                            strMsg = "SP_Your Account activated successfully, Please login to provide further information"
                        });
                    }
                    else
                    {
                        ActivationOTPFailedAttempts++;
                        ViewBag.ShowErrorMessage = "True";
                        if (ActivationOTPFailedAttempts == 1)
                        {                            
                            ViewBag.ShowerrorMessageDescription = "Failed to validate OTP. 2 attempts remaining.";
                        }
                        else if (ActivationOTPFailedAttempts == 2)
                        {
                            ViewBag.ShowerrorMessageDescription = "Failed to validate OTP. 1 attempt remaining.";
                        }
                        else
                        {
                            ViewBag.ShowerrorMessageDescription = "Failed to validate OTP. Please contact us on 267 364 7700, or info@loans.com for further support.";
                        }
                        return View("OTPCheck", model);
                    }
                    
                }

            }
            catch (Exception e)
            {
                errorLogger.Info("ActivationController=>Done" + e.Message + " " + e.StackTrace);
                return RedirectToAction("Index", new { generateSecret = false, error = e.Message ?? string.Empty });
            }
            finally
            {
            }

            return View("Index", new { generateSecret = false });
            

        }


        //[PreserveQueryString]
        public ActionResult OTPCheck(string error = null)
        {
            //string format = "yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss";
            //string format2 = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'";
            //bool linkActive = false; //is link used

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.ShowErrorMessage = "True";
                ViewBag.ShowErrorMessageDescription = error;

            }

            ActivationOTP model = new ActivationOTP();

            try
            {

                string cid = HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get(0);
                string linkID = HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("LinkID");
                //string smDate = Models.Common.Decrypt(HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("sm"), true);
                //DateTime requestDateTime = DateTime.ParseExact(smDate, format, CultureInfo.InvariantCulture);
                //bool linkAlive = CheckLinkAlive(requestDateTime); //is link expired

                model = Models.ActivationOTP.ValidateActivationLink(uint.Parse(cid), linkID);
                model.CellNumber = "";
                using (var proxy = new FINBONDAPIClient())
                {
                    TLogonReply loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], "", "", 1, Convert.ToUInt32(ConfigurationManager.AppSettings["Origin"].ToString()));
                    if (loginReply.ReplyData.ReplyCode == 0)
                    {
                        TClientEnquiryReply clientEnquiry = proxy.ClientEnquiry(loginReply.SessionData, model.IDNumber, true);
                        if (clientEnquiry.ReplyData.ReplyCode == 1)
                        {
                            if (clientEnquiry != null)
                            {
                                model.CellNumber = clientEnquiry.ClientData.MobileNumber;

                                //if (!string.IsNullOrEmpty(clientEnquiry.ClientData.UpdateDateTime))
                                //{
                                //    DateTime lastResetDateTime = DateTime.ParseExact(clientEnquiry.ClientData.UpdateDateTime, format2, CultureInfo.InvariantCulture);
                                //    if (requestDateTime > lastResetDateTime)
                                //    {
                                //        linkActive = true;
                                //        //clientEnquiry.ClientData.UpdateDateTime = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                                //    }
                                //}
                                //else
                                //{
                                //    Console.WriteLine("Failed");
                                //}
                            }
                        }
                    }
                }

                if (model.replyData.ReplyCode != 1)
                {
                    ViewBag.ShowErrorMessage = "True";
                    ViewBag.ShowErrorMessageDescription = model.replyData.ReplyMessage;
                    return View();
                }

                //*****************check if user already used this activation link
                //*****************check if user already used this activation link
                using (var proxy = new FINBONDAPIClient())
                {
                    //TLogonReply loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], "", "", 1, 2043034858);
                    //if (loginReply.ReplyData.ReplyCode == 0)
                    //{
                    TClientEnquiryReply clientEnquiry = proxy.ClientEnquiry(Common.getOperatorSessionData(), model.IDNumber, true);
                    if (clientEnquiry.ReplyData.ReplyCode == 1)
                    {
                        if (clientEnquiry != null)
                        {
                            if (!string.IsNullOrEmpty(clientEnquiry.ClientData.GoogleTwoFactorSecret))
                            {
                                //ViewBag.ShowErrorMessage = "True";
                                //ViewBag.ShowErrorMessageDescription = "The link is already used";
                                return RedirectToAction("Index", "Home", new { strMessage = "SE_The link is already used" });
                                //return View();
                            }
                        }
                    }

                    //**********************check link is expired or not**************************************
                    TReplyData reply=  proxy.ValidateActivationGUID(Common.getOperatorSessionData(), linkID);
                    if(reply.ReplyCode==0)
                    {
                        return RedirectToAction("Index", "Home", new { strMessage = "SE_The link is expired" });
                    }
                    //}
                }
                //if (linkAlive && linkActive)
                //{
                TempData["activationModal"] = model;

                int otpNumber = Common.GenerateOTP();
                //model.OTP = otpNumber.ToString();

                ActivationOTP = otpNumber.ToString();
                ActivationOTPFailedAttempts = 0;


                wLogger.Info("OTP request");
                FinService.FinServiceClient client = new FinService.FinServiceClient();
                client.SendSMS(model.CellNumber, "FINBOND CREDIT: Your OTP is " + otpNumber + ". Please enter this OTP to continue. Call 086 000 4249 for assistance", "FinLoan");
                client.Close();
                wLogger.Info("OTP SENT TO:" + model.CellNumber + "with OTP " + otpNumber + " AT: " + DateTime.Now);

                return View(model);
                //}

                //ViewBag.ShowErrorMessage = "True";
                //ViewBag.ShowErrorMessageDescription = "An error occurred while validating your pending registration. Please try again later, or contact the call center.";
                //return View(model);

            }
            catch (Exception ex)
            {
                errorLogger.Info("ActivationController=>OTPCheck" + ex.Message + " " + ex.StackTrace);
                ViewBag.ShowErrorMessage = "True";
                ViewBag.ShowErrorMessageDescription = "An error occurred while validating your pending registration. Please try again later, or contact the call center.";
                return View();
            }
        }

        private static bool CheckLinkAlive(DateTime created)
        {
            DateTime currentTime = DateTime.Now.AddMinutes(10);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PreserveQueryString]
        public ActionResult Done(Activation model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //if (TimeBasedOneTimePassword.IsValid(TwoFactorSecret, model.ConfirmTwoFactorCode))
                    //{
                    Activation.SetClientActivation(model.ClientCode, "", null);
                    ConfigurationManager.AppSettings["OTPAttemptsMade"] = null;

                    return RedirectToAction("Index", "Login", new
                    {
                        strMsg = "SP_Your Account activated successfully, Please login to provide further information"
                    });
                    //}
                    //else
                    //{
                    //    return RedirectToAction("Index", new { generateSecret = false, error = "Two factor code is incorrect" });
                    //}
                }
            }
            catch (Exception e)
            {
                errorLogger.Info("ActivationCotnroller=>Done" + e.Message + " " + e.StackTrace);
                return RedirectToAction("Index", new { generateSecret = false, error = e.Message ?? string.Empty });
            }
            finally
            {
            }

            return View("Index", new { generateSecret = false });
        }
    }
}