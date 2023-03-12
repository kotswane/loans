using log4net;
using Newtonsoft.Json;
using StaffLoans.LoanAPI;
using StaffLoans.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoFactor;

namespace StaffLoans.Controllers
{
    public class LoginController : BaseController
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");

        // GET: Login
        public ActionResult Index( string strMsg)
        {
            //ShowMessages("SE_TESTING");
            if (!string.IsNullOrEmpty(strMsg))
                ShowMessages(strMsg);
            return View();
        }

        #region Login
        //------------------------------------------Log On--------------------------------------------------------------
        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        public ActionResult LoginUser(LoginModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var proxy = new LoanAPI.FINBONDAPIClient())
                    {
                        wLogger.Info("ClientLogon request");
                        TClientLogonReply z = proxy.ClientLogon(model.IDNumber,Common.Encrypt( model.Password,true), Convert.ToUInt32(ConfigurationManager.AppSettings["Origin"].ToString()));
                        wLogger.Info("ClientLogon response" + JsonConvert.SerializeObject(z));
                        if (z.ReplyData.ReplyCode == 1)
                        {
                            if (z != null && !string.IsNullOrEmpty(z.ClientData.GoogleTwoFactorSecret))
                            {
                                //THE DATE TIME PARSE BELOW IS TEMPORARY TO ALLOW CODE TO COMPILE. REMOVE THIS ONCE THE API DATETIME RETURN FORMAT HAS BEEN CONFIRMED.
                                if ((z.ClientData.LastLoginAttemptUtc != null) && DateTime.Parse(z.ClientData.LastLoginAttemptUtc) > DateTime.UtcNow - TimeSpan.FromSeconds(1))
                                {
                                    System.Threading.Thread.Sleep(5000);
                                }

                                z.ClientData.LastLoginAttemptUtc = DateTime.UtcNow.ToString();
                                /////SAVE TO DB
                                wLogger.Info("ClientLoginAndSecretUpdate request");
                                var updateLoginAttempt = proxy.ClientLoginAndSecretUpdate(z.SessionData, z.ClientData.ClientCode, null,z.ClientData.LastLoginAttemptUtc);
                                wLogger.Info("ClientLoginAndSecretUpdate response" + JsonConvert.SerializeObject(updateLoginAttempt));
                                if (updateLoginAttempt.ReplyCode != 1)
                                {
                                    //log to db
                                }

                                //if (TimeBasedOneTimePassword.IsValid(z.ClientData.GoogleTwoFactorSecret, model.TwoFactorCode))
                                //{
                                    //Redirect to Home Page after successful login.
                                    //***************set session variables and then redirect **********************
                                    Common.ClientLoginSessionId = z.SessionData.SessionID;
                                    Common.LoggedInClientCode =Convert.ToInt32(  z.ClientData.ClientCode);
                                    Common.LoggedInClientUSN = "0";
                                    Common.LoggedInClientName = z.ClientData.FirstName;
                                    Common.LoggedInClientSurName = z.ClientData.LastName;
                                    Common.LoggedInClientEmailAddress = z.ClientData.EMailAddress;
                                    Common.LoggedInClientIDNumber = z.ClientData.IDNumber;
                                    Common.LoggedInClientContactNo = z.ClientData.MobileNumber;
                                    Common.LoggedInClientAddress = z.ClientData.ResAddressStreet + " " + z.ClientData.ResAddressSuburb;
                                    Common.LoggedInClientAddress2 = z.ClientData.ResAddressProvince + " " + z.ClientData.ResAddressCountry + " " + z.ClientData.ResAddressPostalCode;

                                    //var a = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                                    LoanAPI.TClientEnquiryReply clientReply = new LoanAPI.TClientEnquiryReply();
                                    wLogger.Info("ClientEnquiry Request");
                                    clientReply = proxy.ClientEnquiry(Common.getClientSessionData(), Common.LoggedInClientIDNumber, false);
                                    wLogger.Info("ClientEnquiry Response" + JsonConvert.SerializeObject(clientReply));
                                    if (clientReply.ExistingLoanData != null && clientReply.ExistingLoanData.Count() > 0)
                                    {
                                        Common.ClientLoansExists = true;
                                    }
                                    return RedirectToAction("Index", "welcome");
                                //}
                                //else
                                //{
                                //    ViewBag.ShowErrorMessage = "True";
                                //    ViewBag.ShowErrorMessageDescription = "Invalid Credentials";
                                //}
                            }
                            else
                            {
                                ViewBag.ShowErrorMessage = "True";
                                ViewBag.ShowErrorMessageDescription = "Invalid Credentials";
                            }
                        }
                        else
                        {
                            ViewBag.ShowErrorMessage = "True";
                            ViewBag.ShowErrorMessageDescription = "Invalid Credentials";
                        }
                    }
                }
            }
            catch (Exception e) {
                errorLogger.Info("LoginController=>LoginUser" + e.Message + " " + e.StackTrace);
            }

            return View ("Index");
        }

        //[HttpPost]
        //public void LogOnAsync(LogOnModel model, string returnUrl)
        //{
        //    AsyncManager.OutstandingOperations.Increment();
        //    AsyncManager.Parameters["task"] = Task.Factory.StartNew(() => { DoLogOn(model, returnUrl); });
        //}

        //public ActionResult LogOnCompleted(Task task, string returnUrl, string action, string controller, LogOnModel model)
        //{
        //    try
        //    {
        //        task.Wait();
        //    }
        //    catch (AggregateException ex)
        //    {
        //        Exception baseException = ex.GetBaseException();

        //        if (baseException is OneTimePasswordException)
        //        {
        //            model = new LogOnModel();
        //            ModelState.AddModelError("", "This two factor code has already been used. Please wait for the next code to be generated and try again.");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    if (returnUrl != null)
        //    {
        //        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
        //        return Redirect(returnUrl);
        //    }
        //    else if (action != null && controller != null)
        //    {
        //        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
        //        return RedirectToAction(action, controller);
        //    }
        //    else
        //    {
        //        return View(model);
        //    }
        //}

        //public ActionResult LogOff()
        //{
        //    FormsAuthentication.SignOut();

        //    return RedirectToAction("Index", "Home");
        //}
        //------------------------------------------Log On--------------------------------------------------------------
        #endregion

        public ActionResult Logout()
        {
            ClearAllLoggedInSessions();
            return View("Index");
        }
        public void ClearAllLoggedInSessions()
        {
            Session.Abandon();
            Session.RemoveAll();

            //*********************************security fix**********************************
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            //*********************************security fix**********************************
            //CommonMaster.MerchantId = Guid.Empty;
            //CommonMaster.MerchantName = "";
        }
    }
}