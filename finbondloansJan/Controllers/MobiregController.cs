using log4net;
using Newtonsoft.Json;
using StaffLoans.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace StaffLoans.Controllers
{
    public class MobiregController : BaseController
    {

        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");

        public ActionResult Index(string strErrorMsg)
        {
            StaffLoans.Models.PersonalInformation user = new Models.PersonalInformation();
            //wLogger.Info("Staff Loan Activation: ");
            if (!string.IsNullOrEmpty(strErrorMsg))
                ShowMessages(strErrorMsg);

            try
            {
                LoanAPI.TConnectionCheckReply conReply = new LoanAPI.TConnectionCheckReply();

                if (TempData["MobiRegistrationModel"] != null)
                {
                    user = (PersonalInformation)TempData["MobiRegistrationModel"];
                }
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    //wLogger.Info("OperatorLogon request");
                    // var a = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], "", "", 1,Convert.ToUInt32(2043034858));
                    //var reply=proxy.LoanEnquiry(a.SessionData, 1140);

                    //proxy.loanco
                    //LoanAPI.TLoanEnquiryReply
                    //wLogger.Info("OperatorLogon response" + JsonConvert.SerializeObject(a));
                    wLogger.Info("ConnectionCheck request");
                    conReply = proxy.ConnectionCheck(1, 1, Convert.ToUInt32(ConfigurationManager.AppSettings["Origin"].ToString()));
                    //proxy.LoanPreAuth
                    wLogger.Info("ConnectionCheck response" + JsonConvert.SerializeObject(conReply));
                }
                if (conReply != null)
                {
                    if (conReply.LookupValues != null)
                    {
                        #region Titles
                        user.TitleList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "TITLE").ToList()
                                                select new TitleListItem()
                                                {
                                                    TitleId = r.Code,
                                                    TitleDesc = r.Description
                                                }
                                          ).ToList();

                        #endregion

                       

                        #region CountryOfBirth
                        user.CountryOfBirthList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "COUNTRY OF BIRTH").ToList()
                                                select new CountryOfBirthItem()
                                                {
                                                    COBId = r.Code,
                                                    COBDesc = r.Description
                                                }
                                          ).ToList();
                        user.CountryOfBirth = 10;

                        #endregion

                    

                        #region EmploymentType
                        user.EmploymentTypeList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "EMPLOYMENT TYPE").ToList()
                                                select new EmploymentType()
                                                {
                                                    EmploymentTypeId = r.Code,
                                                    EmploymentTypeDesc = r.Description
                                                }
                                          ).ToList();
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger.Info("RegisterController => Index" + ex.Message + " " + ex.StackTrace);
            }


            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterStep1(StaffLoans.Models.PersonalInformation user, string sendOTP, string saveData)
        {
            try
            {
                ////****************call operator logon to create session
                //if (CaptchaMvc.HtmlHelpers.CaptchaHelper.IsCaptchaValid(this, "Invalid Captcha"))
                //{
           
                    if (Common.getOperatorSessionData().SessionID != "")
                    {
                        //**********************call client enquiry to check if idnumber already exists
                        LoanAPI.TClientEnquiryReply clientReply = new LoanAPI.TClientEnquiryReply();
                        LoanAPI.TReplyData preAuthReply = new LoanAPI.TReplyData();
                        using (var proxy = new LoanAPI.FINBONDAPIClient())
                        {
                            wLogger.Info("ClientEnquiry request " + user.IDNumber);
                            clientReply = proxy.ClientEnquiry(Common.getOperatorSessionData(), user.IDNumber, true);
                            wLogger.Info("ClientEnquiry response" + JsonConvert.SerializeObject(clientReply));
                        }
                        if (clientReply.ReplyData.ReplyCode == 1)
                        {
                            //*************if already exists, display message to the user saying IDNumber already exists
                            TempData["RegistrationModel"] = user;
                            return RedirectToAction("Index", new
                            {
                                strErrorMsg = "ID Number already exists"
                            });
                        }
                        else
                        {
                            //**************chk user passes security check for risk matrix, Sanction Screening,PEP Screening
                            using (var proxy = new LoanAPI.FINBONDAPIClient())
                            {
                                wLogger.Info("LoanPreAuth request " + user.IDNumber + " " + user.FirstName + " " + user.LastName + " " + Convert.ToInt32(user.LoanAmount) * 100 + " " + user.CountryOfBirth + " " + user.EmploymentTypeId);
                                preAuthReply = proxy.LoanPreAuth(Common.getOperatorSessionData(), user.IDNumber, user.FirstName, user.LastName, Convert.ToInt32(user.LoanAmount) * 100, user.CountryOfBirth, user.EmploymentTypeId);
                                wLogger.Info("LoanPreAuth response" + JsonConvert.SerializeObject(preAuthReply));
                            }
                            if (preAuthReply.ReplyCode == 0)
                            {
                                //**************insert this employee information*****************
                                LoanAPI.TClientData emp = new LoanAPI.TClientData();
                                LoanAPI.TInsUpdReply insertReply = new LoanAPI.TInsUpdReply();
                                emp.FirstName = user.FirstName;
                                emp.MiddleName = user.MiddleName;
                                emp.LastName = user.LastName;
                                emp.IDNumber = user.IDNumber;
                                emp.MobileNumber = user.MobileNumber;
                                emp.Nationality = user.Nationality;
                                if (user.DateOfBirth != null)
                                {
                                    emp.DOB = Convert.ToUInt32(user.DateOfBirth.Replace("/", ""));
                                }
                                emp.Title = user.Title;
                                emp.MaritalStatus = user.MaritalStatus;
                                emp.CountryOfBirth = user.CountryOfBirth;

                                emp.WorkNumber = user.WorkNumber;
                                emp.EMailAddress = user.EmailAddress;
                                emp.AlternateContactNumber = user.AlternateContactNumber;
                                emp.Language = user.Language;

                                emp.AlternateContactFirstName = user.AlternateContactFirstName;
                                emp.AlternateContactLastName = user.AlternateContactLastName;
                                if ( user.MarketingPreference == 0)
                                {
                                    emp.MarketingConsent = 0;
                                    emp.MarketingPreference = -1;
                                }
                                else
                                {
                                    emp.MarketingConsent = 1;
                                    emp.MarketingPreference = user.MarketingPreference;
                                }
                                // emp.MarketingConsent = user.MarketingConsent;
                                //emp.MarketingPreference = user.MarketingPreference;
                                //emp.MarketingConsent = 1;
                                emp.ResAddressStreet = user.Street;
                                emp.ResAddressSuburb = user.Suburb;
                                emp.ResAddressProvince = user.Province;
                                emp.ResAddressPostalCode = Convert.ToInt32(user.PostalCode);
                                emp.ResAddressCountry = user.Country;

                                emp.PostalAddressStreet = user.PostalStreet;
                                emp.PostalAddressSuburb = user.PostalSuburb;
                                emp.PostalAddressProvince = user.Province;
                                emp.PostalAddressPostalCode = Convert.ToInt32(user.PostalPostalCode);
                                emp.PostalAddressCountry = user.PostalCountry;
                                emp.ActivationGUID = Guid.NewGuid().ToString();
                                emp.GoogleTwoFactorSecret = Common.Encrypt(user.Password, true);
                                using (var proxy = new LoanAPI.FINBONDAPIClient())
                                {
                                    wLogger.Info("InsUpdClientDetails request " + JsonConvert.SerializeObject(emp));
                                    insertReply = proxy.InsUpdClientDetails(Common.getOperatorSessionData(), emp);
                                    if (insertReply.ReplyData.ReplyCode == 0)
                                    {
                                        wLogger.Info("InsUpdClientDetails response" + insertReply.ReplyData.ReplyMessage);
                                        //*********************send activation eamil *******************
                                        LoanAPI.TReplyData activationReply = new LoanAPI.TReplyData();
                                        wLogger.Info("InsUpdClientActivation request");
                                        activationReply = proxy.InsUpdClientActivation(Common.getOperatorSessionData(), Convert.ToUInt32(insertReply.ClientCode), Common.Encrypt(user.Password, true), emp.ActivationGUID);
                                        wLogger.Info("InsUpdClientActivation response" + JsonConvert.SerializeObject(activationReply));
                                        if (activationReply.ReplyCode == 1)
                                        {
                                            StringBuilder sb = new StringBuilder("");
                                            SendActivationEmail(insertReply.ClientCode, user.EmailAddress, emp.ActivationGUID, emp.FirstName);
                                            return RedirectToAction("Index", "MobiThankYou");
                                        }
                                        else
                                        {
                                            //*****************display error message to the user**************
                                            TempData["MobiRegistrationModel"] = user;
                                            return RedirectToAction("Index", new
                                            {
                                                strErrorMsg = "Technical problem," + activationReply.ReplyMessage
                                            });
                                        }

                                    }
                                    else
                                    {
                                        wLogger.Info("InsUpdClientDetails response" + insertReply.ReplyData.ReplyCode + " " + insertReply.ReplyData.ReplyMessage);
                                        //*****************display error message to the user**************
                                        TempData["MobiRegistrationModel"] = user;
                                        return RedirectToAction("Index", new
                                        {
                                            strErrorMsg = "Technical problem," + insertReply.ReplyData.ReplyMessage
                                        });
                                    }
                                }
                            }
                            else
                            {
                                TempData["MobiRegistrationModel"] = user;
                                return RedirectToAction("Index", new
                                {
                                    strErrorMsg = "Technical problem," + preAuthReply.ReplyMessage
                                });
                            }

                        }
                    }
                    else
                    {
                        TempData["MobiRegistrationModel"] = user;
                        return RedirectToAction("Index", new
                        {
                            strErrorMsg = "Invalid Session"
                        });
                    }
                //}
                //else
                //{
                //    TempData["MobiRegistrationModel"] = user;
                //    return RedirectToAction("Index", new
                //    {
                //        strErrorMsg = "Invalid Captcha"
                //    });
                //}
            }
            catch (Exception ex)
            {
                errorLogger.Info("MobiRegisterCotnroller =>RegisterStep1" + ex.Message + " " + ex.StackTrace);
            }
            TempData["MobiRegistrationModel"] = user;
            return RedirectToAction("Index", new
            {
                strErrorMsg = "Error occured during registration"
            });
            //return View();
        }

        public void SendActivationEmail(int iClientID, string strEmailId, string activationGUID, string strUserName)
        {
            StringBuilder sb = new StringBuilder("");
            //sb.Append("<br/>");
            //sb.Append("<br/>");
            //sb.Append("Hi " + strUserName);
            //sb.Append("<br/>");
            //sb.Append("Please click on below URL or Alternatively, you can copy this link and paste it into your browser to activate your account:");
            //sb.Append("<Br/>");

            DateTime currentTime = DateTime.Now;
            DateTime bufferTime = currentTime.AddMinutes(10);
            string submissionDate = bufferTime.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss", CultureInfo.InvariantCulture);
            string encrSubDate = Common.Encrypt(submissionDate, true);
            string encoSubDate = HttpUtility.UrlEncode(encrSubDate, Encoding.ASCII);

            string strURL = ConfigurationManager.AppSettings["ActivationURL"].ToString() + "?CID=" + iClientID + "&LinkID=" + activationGUID + "&sm=" + encoSubDate + "";

            //sb.Append(strURL);
            //sb.Append("<Br/>");
            //sb.Append("<Br/>");
            //sb.Append("Thank You");
            //sb.Append("<Br/>");
            //sb.Append("<Br/>");

            sb.Append("<map name='mapFooLink'>");
            sb.Append("	<area shape='rect' coords='133,21,283,62' href='http://www.net1.com/' />");
            //sb.Append("<area shape='rect' coords='429,29,528,48' href='http://www.net1.com/' />");
            sb.Append("</map>");
            sb.Append("<table width=\"960px\" cellpadding=\"0\" cellspacing=\"0\" style=\"color:#9EA4A9;\">");
            sb.Append("<tr>");
            sb.Append("<td ><img  usemap=\"#mapFooLink\" src=\"cid:imgHEADER\" width=\"960\" height=\"216\" alt=\"FINBOND\" /></td>");
            sb.Append("</tr>");
            //sb.Append("<tr><td style=\"background-color:#a7c1e4;height:4px\"></td></tr>");
            sb.Append("<tr><td style =\"font-family:Futura Lt BT;color:#4a4a4a\">");
            // sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:13pt;color:#9EA4A9\">Welcome " + strUserName + ",</div>");
            // sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9;\">Thank you for registering on FINBOND Loans.</ div>");
            //sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div  style=\"font-family:arial;font-size:10pt;color:#9EA4A9;\">To activate your accoount, please click on the link below or you can copy and paste the link into you browser.</ div>");
            sb.Append("<br />");
            sb.Append(strURL);
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<b>Once your account has been activated, please remember to update your address details on your profile page.</b>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("You currently cannot log in using your mobile device. Please use a desktop computer or laptop with internet connection.");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-weight:bold;font-size:10pt;color:#003F7D\">FINBOND LOANS TEAM</div>");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">If you did not apply for this service, please call or email us to alert us immediately.</div>");
            sb.Append("<br />");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td ><img  src=\"cid:imgFOOTER\" width=\"960\" height=\"489\" alt=\"FINBOND\" /></td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            List<string> lst = new List<string>();
            lst.Add(strEmailId);
            Emails.SendEmail(sb, "Finbond Loan Account Activation", lst);
            wLogger.Info("Finbond Loan Account Activation:  " + lst + sb.ToString());
        }
    }
}