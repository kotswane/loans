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
    public class RegisterController : BaseController
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

                if (TempData["RegistrationModel"] != null)
                {
                    user = (PersonalInformation)TempData["RegistrationModel"];
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

                        #region MaritalStatus
                        user.MaritalStatusList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "MARITAL STATUS").ToList()
                                                select new MaritalStatusItem()
                                                {
                                                    MartialStatusId = r.Code,
                                                    MartialStatuseDesc = r.Description
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

                        #region Nationality
                        user.NationalityList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "NATIONALITY").ToList()
                                                select new NationalityItem()
                                                {
                                                    NationalityId = r.Code,
                                                    NationalityDesc = r.Description
                                                }
                                          ).ToList();

                        #endregion

                        #region Country
                        CountryItem citem = new CountryItem();
                        citem.CountryId = "South Africa";
                        citem.CountryDesc = "South Africa";
                        user.CountryList = new List<Models.CountryItem>();
                        user.CountryList.Add(citem);

                        #endregion

                        #region province
                        user.ProvinceList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "PROVINCE").ToList()
                                                select new ProvinceItem()
                                                {
                                                    ProvinceId = r.Code,
                                                    ProvinceDesc = r.Description
                                                }
                                          ).ToList();

                        #endregion

                        #region Language
                        user.LanguageList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "LANGUAGE").ToList()
                                                select new LanguageItem()
                                                {
                                                    LanguageId = r.Code,
                                                    LanguageDesc = r.Description
                                                }
                                          ).ToList();

                        #endregion

                        #region MarketingPreference
                        user.MarketingPreferenceList = (
                                                from r in conReply.LookupValues.Where(m => m.LookupName == "MARKETING PREFERENCE").ToList()
                                                select new MarketingPreferenceItem()
                                                {
                                                    MarketingPreId = r.Code,
                                                    MarketingPreDesc = r.Description
                                                }
                                          ).ToList();

                        MarketingPreferenceItem mp = new MarketingPreferenceItem();
                        mp.MarketingPreId = 0;
                        mp.MarketingPreDesc = "None";
                        user.MarketingPreferenceList.Insert(0, mp);
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
            catch(Exception ex)
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
                //****************call operator logon to create session
                if (CaptchaMvc.HtmlHelpers.CaptchaHelper.IsCaptchaValid(this, "Invalid Captcha"))
                {
                    //LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
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

                    if (Common.getOperatorSessionData().SessionID != "")
                    {
                        //**********************call client enquiry to check if idnumber already exists
                        LoanAPI.TClientEnquiryReply clientReply = new LoanAPI.TClientEnquiryReply();
                        LoanAPI.TReplyData preAuthReply = new LoanAPI.TReplyData();
                        using (var proxy = new LoanAPI.FINBONDAPIClient())
                        {
                            wLogger.Info("ClientEnquiry request "  + user.IDNumber);
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
                                preAuthReply = proxy.LoanPreAuth(Common.getOperatorSessionData(), user.IDNumber, user.FirstName, user.LastName,Convert.ToInt32( user.LoanAmount)*100, user.CountryOfBirth, user.EmploymentTypeId);
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
                                emp.DOB = Convert.ToUInt32(user.DateOfBirth.Replace("/", ""));
                                emp.Title = user.Title;
                                emp.MaritalStatus = user.MaritalStatus;
                                emp.CountryOfBirth = user.CountryOfBirth;

                                emp.WorkNumber = user.WorkNumber;
                                emp.EMailAddress = user.EmailAddress;
                                emp.AlternateContactNumber = user.AlternateContactNumber;
                                emp.Language = user.Language;

                                emp.AlternateContactFirstName = user.AlternateContactFirstName;
                                emp.AlternateContactLastName = user.AlternateContactLastName;
                                if (user.MarketingPreference == 0)
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
                                    wLogger.Info("InsUpdClientDetails request " + JsonConvert.SerializeObject(emp ));
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
                                            return RedirectToAction("Index", "ThankYou");
                                        }
                                        else
                                        {
                                            //*****************display error message to the user**************
                                            TempData["RegistrationModel"] = user;
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
                                        TempData["RegistrationModel"] = user;
                                        return RedirectToAction("Index", new
                                        {
                                            strErrorMsg = "Technical problem," + insertReply.ReplyData.ReplyMessage
                                        });
                                    }
                                }
                            }
                            else
                            {
                                TempData["RegistrationModel"] = user;
                                return RedirectToAction("Index", new
                                {
                                    strErrorMsg = "Technical problem," + preAuthReply.ReplyMessage
                                });
                            }

                        }
                    }
                    else
                    {
                        TempData["RegistrationModel"] = user;
                        return RedirectToAction("Index", new
                        {
                            strErrorMsg = "Invalid Session"
                        });
                    }
                }
                else
                {
                    TempData["RegistrationModel"] = user;
                    return RedirectToAction("Index", new
                    {
                        strErrorMsg = "Invalid Captcha"
                    });
                }
            }
            catch(Exception ex)
            {
                errorLogger.Info("RegisterCotnroller =>RegisterStep1" + ex.Message + " " + ex.StackTrace);
            }
            TempData["RegistrationModel"] = user;
            return RedirectToAction("Index", new
            {
                strErrorMsg = "Error occured during registration"
            });
            //return View();
        }

        public void SendActivationEmail(int iClientID, string strEmailId, string activationGUID, string strUserName)
        {
            StringBuilder sb = new StringBuilder("");

            DateTime currentTime = DateTime.Now;
            DateTime bufferTime = currentTime.AddMinutes(10);
            string submissionDate = bufferTime.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss", CultureInfo.InvariantCulture);
            string encrSubDate = Common.Encrypt(submissionDate, true);
            string encoSubDate = HttpUtility.UrlEncode(encrSubDate, Encoding.ASCII);

            string strURL = ConfigurationManager.AppSettings["ActivationURL"].ToString() + "?CID=" + iClientID + "&LinkID=" + activationGUID + "&sm=" + encoSubDate + "";
            
            sb.Append("<table width=\"960px\" cellpadding=\"0\" cellspacing=\"0\" style=\"color:#9EA4A9;\">");
            sb.Append("<tr>");
            sb.Append("<td ><img  usemap=\"#mapFooLink\" src=\"cid:imgHEADER\" width=\"960\" height=\"216\" alt=\"FINBOND\" /></td>");
            sb.Append("</tr>");
            sb.Append("<tr><td style =\"font-family:Futura Lt BT;color:#4a4a4a\">");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:13pt;color:#9EA4A9\">Dear " + strUserName + ",</div>");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9;\">Thank you for registering on Finbond Credit.</ div>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div  style=\"font-family:arial;font-size:10pt;color:#9EA4A9;\">To activate your account and start your loan application please verify your email by clicking the below link or you can copy and paste the URL into your web browser.</ div>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append(strURL);
            sb.Append("<br />");
            sb.Append("<br />");

            sb.Append("<div style=\"font-family:arial;font-weight:bold;font-size:10pt;color:#003F7D; display:inline-block\">If you would like to find out more about Finbond Credit, please refer to our &nbsp;</div>");
            sb.Append("<a style=\"display:inline-block\" href=\"https://www.finbondcredit.co.za/faq \"> [FAQ] </a>");
            sb.Append("<div style=\"font-family:arial;font-weight:bold;font-size:10pt;color:#003F7D; display:inline-block\">&nbsp; or contact us by phone or email.</div>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">Call Centre: 086 000 4249</div>");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">Email: loans@finbond.co.za  </div>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">Regards,</div>");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">Finbond Credit Team</div>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">If you did not apply for this services please ignore this or email:loans@finbond.co.za  or contact our call center: 086 000 4249 to log a case</div>");
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
