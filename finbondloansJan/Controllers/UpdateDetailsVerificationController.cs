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
    public class UpdateDetailsVerificationController : BaseController
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");
        private const string SESS_UPDATE_OTP = "UpdateOTP";
        private const string SESS_UPDATE_OTP_FAILED_ATTEMPTS = "UpdateOTPFailedAttempts";
        private static string UpdateOTP
        {
            get
            {
                if (System.Web.HttpContext.Current.Session[SESS_UPDATE_OTP] != null)
                    return System.Web.HttpContext.Current.Session[SESS_UPDATE_OTP].ToString();
                else
                    return "";
            }
            set
            {
                System.Web.HttpContext.Current.Session[SESS_UPDATE_OTP] = value;
            }
        }

        private static int UpdateOTPFailedAttempts
        {
            get
            {
                if (System.Web.HttpContext.Current.Session[SESS_UPDATE_OTP_FAILED_ATTEMPTS] != null)
                    return (int)System.Web.HttpContext.Current.Session[SESS_UPDATE_OTP_FAILED_ATTEMPTS];
                else
                    return 0;
            }
            set
            {
                System.Web.HttpContext.Current.Session[SESS_UPDATE_OTP_FAILED_ATTEMPTS] = value;
            }

        }

        public ActionResult FailedOTP()
        {
            return RedirectToAction("Index", "Welcome", new { strMsg = "You have entered your OTP incorrectly 3 times. Please contact us on 267 364 7700, or info@loans.com to continue setting up your account." });
        }

        public ActionResult Index(bool generateSecret = true, string error = null, string code = null, string id = null)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.ShowErrorMessage = "True";
                ViewBag.ShowErrorMessageDescription = error;
                //return View();
            }

            Update model = new Update();

            try
            {
                if (code != null && id != null)
                {
                    UpdateOTP = null;
                    UpdateOTPFailedAttempts = 0;

                    model = Update.ValidateUpdateLink(uint.Parse(code), id);
                }
                else
                {
                    return RedirectToAction("Index", "Welcome", new { strMsg = "SE_Failed to validate link. Please try again." });
                }


                if (model.replyData.ReplyCode != 1)
                {
                    ViewBag.ShowErrorMessage = "True";
                    ViewBag.ShowErrorMessageDescription = model.replyData.ReplyMessage;
                    return View();
                }
                TempData["updateModal"] = model;

                return View(model);
            }
            catch (Exception ex)
            {
                errorLogger.Info("UpdateCotnroller=>Index" + ex.Message + " " + ex.StackTrace);
                ViewBag.ShowErrorMessage = "True";
                ViewBag.ShowErrorMessageDescription = "An error occurred while validating your pending update. Please try again later, or contact the call center.";
                return View();
            }
        }

        //[PreserveQueryString]
        public ActionResult OTPCheck(string error = null)
        {


            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.ShowErrorMessage = "True";
                ViewBag.ShowErrorMessageDescription = error;

            }

            UpdateOTP model = new UpdateOTP();























            try
            {

                string cid = HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get(0);
                string linkID = HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("LinkID");
                //string email = HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("e");
                //string cell = HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("c");

                string email = Models.Common.Decrypt(HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("e"), true);
                string cell = Models.Common.Decrypt(HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("c"), true);
                string originalCell = Models.Common.Decrypt(HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("om"), true);
                string idNumber = Models.Common.Decrypt(HttpUtility.ParseQueryString(HttpContext.Request.Url.OriginalString).Get("i"), true);

                model = Models.UpdateOTP.ValidateUpdateLink(uint.Parse(cid), linkID);
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
                                model.NewCell = cell;
                                model.NewEmail = email;
                                model.IDNumber = idNumber;
                                
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
                
                TempData["updateModal"] = model;

                int otpNumber = Common.GenerateOTP();

                UpdateOTP = otpNumber.ToString();
                UpdateOTPFailedAttempts = 0;

                wLogger.Info("OTP request");
                FinService.FinServiceClient client = new FinService.FinServiceClient();
                client.SendSMS(originalCell, "FINBOND CREDIT: You are about to make an update to your profile. OTP is " + otpNumber + ". Please enter this OTP to continue. Call 086 000 4249 for assistance", "FinLoan");
                client.Close();
                wLogger.Info("OTP SENT TO:" + model.CellNumber + "with OTP " + otpNumber + " AT: " + DateTime.Now);

                return View(model);

            }
            catch (Exception ex)
            {
                errorLogger.Info("UpdateDetailsVerificationController=>OTPCheck" + ex.Message + " " + ex.StackTrace);
                ViewBag.ShowErrorMessage = "True";
                ViewBag.ShowErrorMessageDescription = "An error occurred while validating your pending update. Please try again later, or contact the call center.";
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
        public ActionResult Done(Update model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //if (TimeBasedOneTimePassword.IsValid(TwoFactorSecret, model.ConfirmTwoFactorCode))
                    //{
                    Update.SetClientUpdate(model.ClientCode, "", null);
                    ConfigurationManager.AppSettings["OTPAttemptsMade"] = null;

                    return RedirectToAction("Index", "Welcome", new
                    {
                        strMsg = "SP_Your Account updated successfully."
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
                errorLogger.Info("UpdateCotnroller=>Done" + e.Message + " " + e.StackTrace);
                return RedirectToAction("Index", new { generateSecret = false, error = e.Message ?? string.Empty });
            }
            finally
            {
            }

            return View("Index", new { generateSecret = false });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        //[PreserveQueryString]
        public ActionResult OTPPrompt(UpdateOTP model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (UpdateOTP == model.CellNumberOTPPrompt && UpdateOTPFailedAttempts <= 3)
                    {

                       // UpdateEmailOrCell(model.NewEmail, model.NewCell, model.IDNumber);
                        return RedirectToAction("UpdateEmailOrCell", "UpdateDetailsVerification", new { email = model.NewEmail, cell = model.NewCell, id = model.IDNumber });

                    }
                    else
                    {
                        UpdateOTPFailedAttempts++;
                        ViewBag.ShowErrorMessage = "True";
                        if (UpdateOTPFailedAttempts == 1)
                        {
                            ViewBag.ShowerrorMessageDescription = "Failed to validate OTP. 2 attempts remaining.";
                        }
                        else if (UpdateOTPFailedAttempts == 2)
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
                errorLogger.Info("UpdateDetailsVerificationController=>Done" + e.Message + " " + e.StackTrace);
                return RedirectToAction("Index", new { generateSecret = false, error = e.Message ?? string.Empty });
            }

            return View("Index", new { generateSecret = false });


        }


        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateEmailOrCell(string email, string cell, string id)
        {
            //****************call operator logon to create session
            LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
            try
            {
                //if (Common.ClientLoginSessionId != "")
                //{

                    //**********************call client enquiry to check if idnumber already exists
                    LoanAPI.TClientEnquiryReply clientReply = new LoanAPI.TClientEnquiryReply();

                    //**************insert this employee information*****************
                    LoanAPI.TClientData emp = new LoanAPI.TClientData();
                    LoanAPI.TInsUpdReply insertReply = new LoanAPI.TInsUpdReply();

                    using (var proxy = new LoanAPI.FINBONDAPIClient())
                    {



                        wLogger.Info("ClientEnquiry request");
                        clientReply = proxy.ClientEnquiry(Common.getOperatorSessionData(),id, true);
                        wLogger.Info("ClientEnquiry response" + JsonConvert.SerializeObject(clientReply));
                        if (clientReply != null)
                        {
                            if (clientReply.ReplyData.ReplyCode == 1)
                            {
                                emp = clientReply.ClientData;
                            }
                            else
                            {
                                ShowMessages("User does not exists");
                            }
                        }

                        emp.MobileNumber = cell;

                        emp.EMailAddress = email;





                        wLogger.Info("InsUpdClientDetails request");
                        insertReply = proxy.InsUpdClientDetails(Common.getOperatorSessionData(), emp);
                        wLogger.Info("InsUpdClientDetails response" + JsonConvert.SerializeObject(insertReply));

                        if (insertReply.ReplyData.ReplyCode == 0)
                        {
                            //*********************send activation eamil *******************
                            LoanAPI.TReplyData activationReply = new LoanAPI.TReplyData();
                            return RedirectToAction("Index", "Home", new
                            {
                                strMessage = "SP_Profile updated successfully"
                            });
                        }
                        else
                        {
                            //*****************display error message to the user**************
                            return RedirectToAction("Index", "Home", new
                            {
                                strMessage = "Technical problem," + insertReply.ReplyData.ReplyMessage
                            });
                        }
                    }
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Home", new
                //    {
                //        strMessage = "Invalid Session"
                //    });
                //}
            }
            catch (Exception ex)
            {
                errorLogger.Info("UpdateDetailsVerificationController=>UpdateEmailOrCell " + ex.Message + " " + ex.StackTrace);
            }
            return RedirectToAction("Index", "Home", new
            {
                strMessage = "Error occured during update"
            });
        }





    }
}