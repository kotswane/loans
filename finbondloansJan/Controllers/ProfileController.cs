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
    public class ProfileController : BaseController
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");
        private static readonly ILog errorLogger = LogManager.GetLogger("ErrorLoLogger");

        [CustomAuthorize]
        public ActionResult Index(string strErrorMsg)
        {
            StaffLoans.Models.UpdatePersonalInformation user = new Models.UpdatePersonalInformation();
            try
            {
                //wLogger.Info("Staff Loan Activation: ");
                if (!string.IsNullOrEmpty(strErrorMsg))
                    ShowMessages(strErrorMsg);

                LoanAPI.TConnectionCheckReply conReply = new LoanAPI.TConnectionCheckReply();
                //LoanAPI.TLogonReply  logonReply = new LoanAPI.TLogonReply();
                LoanAPI.TClientEnquiryReply clientEnquiryReply = new LoanAPI.TClientEnquiryReply();
                if (TempData["RegistrationModel"] != null)
                {
                    user = (UpdatePersonalInformation)TempData["RegistrationModel"];
                }
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    //wLogger.Info("OperatorLogon request");
                    //logonReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                    ////var reply=proxy.LoanEnquiry(a.SessionData, 1140);
                    //wLogger.Info("OperatorLogon response" + JsonConvert.SerializeObject(logonReply));
                    //proxy.loanco
                    //LoanAPI.TLoanEnquiryReply
                    wLogger.Info("ConnectionCheck request");
                    conReply = proxy.ConnectionCheck(1, 1,Convert.ToUInt32( ConfigurationManager.AppSettings["Origin"].ToString()));
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

                        #endregion

                        #region Papulate ProfileDetails
                        using (var proxy = new LoanAPI.FINBONDAPIClient())
                        {
                            wLogger.Info("ClientEnquiry request");
                            clientEnquiryReply = proxy.ClientEnquiry(Common.getClientSessionData(), Common.LoggedInClientIDNumber, true);
                            wLogger.Info("ClientEnquiry response" + JsonConvert.SerializeObject(clientEnquiryReply));
                            if (clientEnquiryReply != null)
                            {
                                if (clientEnquiryReply.ReplyData.ReplyCode == 1)
                                {
                                    user.ClientCode = clientEnquiryReply.ClientData.ClientCode.ToString();
                                    user.FirstName = clientEnquiryReply.ClientData.FirstName;
                                    user.LastName = clientEnquiryReply.ClientData.LastName;
                                    user.MiddleName = clientEnquiryReply.ClientData.MiddleName;
                                    user.IDNumber = clientEnquiryReply.ClientData.IDNumber;
                                    user.DateOfBirth = clientEnquiryReply.ClientData.DOB.ToString();
                                    user.Title = clientEnquiryReply.ClientData.Title;
                                    user.MaritalStatus = clientEnquiryReply.ClientData.MaritalStatus;
                                    user.EmailAddress = clientEnquiryReply.ClientData.EMailAddress;
                                    user.MobileNumber = clientEnquiryReply.ClientData.MobileNumber;

                                    user.OriginalEmailAddress = clientEnquiryReply.ClientData.EMailAddress;
                                    user.OriginalMobileNumber = clientEnquiryReply.ClientData.MobileNumber;

                                    user.WorkNumber = clientEnquiryReply.ClientData.WorkNumber;
                                    user.AlternateNumber = clientEnquiryReply.ClientData.AlternateContactNumber;
                                    user.Nationality = clientEnquiryReply.ClientData.Nationality;
                                    user.Language = clientEnquiryReply.ClientData.Language;

                                    user.AlternateContactFirstName = clientEnquiryReply.ClientData.AlternateContactFirstName;
                                    user.AlternateContactLastName = clientEnquiryReply.ClientData.AlternateContactLastName;
                                    user.AlternateContactNumber = clientEnquiryReply.ClientData.AlternateContactNumber;
                                    user.MarketingPreference = clientEnquiryReply.ClientData.MarketingPreference;
                                    //  user.Password  = clientEnquiryReply.ClientData.Password;
                                    user.Street = clientEnquiryReply.ClientData.ResAddressStreet;

                                    user.Suburb = clientEnquiryReply.ClientData.ResAddressSuburb;
                                    user.Country = clientEnquiryReply.ClientData.ResAddressCountry;
                                    user.Province = clientEnquiryReply.ClientData.ResAddressProvince;
                                    user.PostalCode = clientEnquiryReply.ClientData.ResAddressPostalCode.ToString();

                                    user.PostalStreet = clientEnquiryReply.ClientData.PostalAddressStreet;

                                    user.PostalSuburb = clientEnquiryReply.ClientData.PostalAddressSuburb;
                                    user.PostalCountry = clientEnquiryReply.ClientData.PostalAddressCountry;
                                    user.PostalProvince = clientEnquiryReply.ClientData.PostalAddressProvince;
                                    user.PostalPostalCode = clientEnquiryReply.ClientData.PostalAddressPostalCode.ToString();
                                }
                                else
                                {
                                    ShowMessages("User does not exists");
                                }
                            }

                        }

                        #endregion
                    }
                }

            }
            catch(Exception ex)
            {
                errorLogger.Info("ProfileController=>Index " + ex.Message + " " + ex.StackTrace);
            }

            TempData["EmailTemp"] = user.EmailAddress;
            TempData["CellNoTemp"] = user.MobileNumber;

            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(StaffLoans.Models.UpdatePersonalInformation user, string sendOTP, string saveData)
        {

            object originalEmail = TempData["EmailTemp"];
            object originalCell = TempData["CellNoTemp"];

            //****************call operator logon to create session
            LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
            try
            {

                if (Common.ClientLoginSessionId != "")
                {
                    //if (originalCell.ToString() != user.MobileNumber || originalEmail.ToString() != user.EmailAddress)
                    //{
                    //    SendUpdateDetailsEmail(Int32.Parse(user.ClientCode), user.EmailAddress, Guid.NewGuid().ToString(), user.FirstName, user.EmailAddress, user.MobileNumber);
                    //    return RedirectToAction("UpdatedDetails", "ThankYou");
                    //}
                    

                        //**********************call client enquiry to check if idnumber already exists
                        LoanAPI.TClientEnquiryReply clientReply = new LoanAPI.TClientEnquiryReply();

                        //**************insert this employee information*****************
                        LoanAPI.TClientData emp = new LoanAPI.TClientData();
                        LoanAPI.TInsUpdReply insertReply = new LoanAPI.TInsUpdReply();
                        emp.ClientCode = Convert.ToUInt32(user.ClientCode);
                        emp.FirstName = user.FirstName;
                        emp.MiddleName = user.MiddleName;
                        emp.LastName = user.LastName;
                        emp.IDNumber = user.IDNumber;
                        //emp.MobileNumber = user.MobileNumber;
                        emp.Nationality = user.Nationality;
                        emp.DOB = Convert.ToUInt32(user.DateOfBirth.Replace("/", ""));
                        emp.Title = user.Title;
                        emp.MaritalStatus = user.MaritalStatus;
                        emp.CountryOfBirth = user.CountryOfBirth;

                        emp.MobileNumber = user.OriginalMobileNumber;
                        emp.EMailAddress = user.OriginalEmailAddress;


                        emp.WorkNumber = user.WorkNumber;
                        //emp.EMailAddress = user.EmailAddress;
                        emp.AlternateContactNumber = user.AlternateNumber;
                        emp.Language = user.Language;

                        emp.AlternateContactFirstName = user.AlternateContactFirstName;
                        emp.AlternateContactLastName = user.AlternateContactLastName;
                        // emp.MarketingConsent = user.MarketingConsent;
                        emp.MarketingPreference = user.MarketingPreference;
                        emp.MarketingConsent = 1;
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
                        emp.GoogleTwoFactorSecret = user.Password;

                    if (originalCell.ToString() != user.MobileNumber || originalEmail.ToString() != user.EmailAddress)
                    {
                        using (var proxy = new LoanAPI.FINBONDAPIClient())
                        {
                            wLogger.Info("InsUpdClientDetails request");
                            insertReply = proxy.InsUpdClientDetails(Common.getClientSessionData(), emp);
                            wLogger.Info("InsUpdClientDetails response" + JsonConvert.SerializeObject(insertReply));
                            if (insertReply.ReplyData.ReplyCode == 0)
                            {
                                //*********************send activation eamil *******************
                                LoanAPI.TReplyData activationReply = new LoanAPI.TReplyData();

                                SendUpdateDetailsEmail(Int32.Parse(user.ClientCode), user.OriginalEmailAddress, Guid.NewGuid().ToString(), user.FirstName, user.EmailAddress, user.MobileNumber, user.OriginalMobileNumber);
                                return RedirectToAction("UpdatedDetails", "ThankYou");
                            }
                            else
                            {
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
                        emp.MobileNumber = user.MobileNumber;
                        emp.EMailAddress = user.EmailAddress;

                        using (var proxy = new LoanAPI.FINBONDAPIClient())
                        {
                            wLogger.Info("InsUpdClientDetails request");
                            insertReply = proxy.InsUpdClientDetails(Common.getClientSessionData(), emp);
                            wLogger.Info("InsUpdClientDetails response" + JsonConvert.SerializeObject(insertReply));
                            if (insertReply.ReplyData.ReplyCode == 0)
                            {
                                //*********************send activation eamil *******************
                                LoanAPI.TReplyData activationReply = new LoanAPI.TReplyData();
                                return RedirectToAction("Index", new
                                {
                                    strErrorMsg = "SP_Profile updated successfully"
                                });
                            }
                            else
                            {
                                //*****************display error message to the user**************
                                TempData["RegistrationModel"] = user;
                                return RedirectToAction("Index", new
                                {
                                    strErrorMsg = "Technical problem," + insertReply.ReplyData.ReplyMessage
                                });
                            }
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
            catch (Exception ex)
            {
                errorLogger.Info("ProfileController=>UpdateProfile " + ex.Message + " " + ex.StackTrace);
            }
            TempData["RegistrationModel"] = user;
            return RedirectToAction("Index", new
            {
                strErrorMsg = "Error occured during update"
            });

            //return View();
        }


















        ////[HttpPost]
        ////[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        //public ActionResult UpdateEmailOrCell(string email, string cell, string id)
        //{
        //    //****************call operator logon to create session
        //    LoanAPI.TLogonReply loginReply = new LoanAPI.TLogonReply();
        //    try
        //    {
        //        if (Common.ClientLoginSessionId != "")
        //        {
                    
        //            //**********************call client enquiry to check if idnumber already exists
        //            LoanAPI.TClientEnquiryReply clientReply = new LoanAPI.TClientEnquiryReply();

        //            //**************insert this employee information*****************
        //            LoanAPI.TClientData emp = new LoanAPI.TClientData();
        //            LoanAPI.TInsUpdReply insertReply = new LoanAPI.TInsUpdReply();
                    


                    
        //            using (var proxy = new LoanAPI.FINBONDAPIClient())
        //            {



        //                wLogger.Info("ClientEnquiry request");
        //                clientReply = proxy.ClientEnquiry(Common.getClientSessionData(), Common.LoggedInClientIDNumber, true);
        //                wLogger.Info("ClientEnquiry response" + JsonConvert.SerializeObject(clientReply));
        //                if (clientReply != null)
        //                {
        //                    if (clientReply.ReplyData.ReplyCode == 1)
        //                    {
        //                        emp = clientReply.ClientData;
        //                    }
        //                    else
        //                    {
        //                        ShowMessages("User does not exists");
        //                    }
        //                }

        //                emp.MobileNumber = cell;

        //                emp.EMailAddress = email;





        //                wLogger.Info("InsUpdClientDetails request");
        //                insertReply = proxy.InsUpdClientDetails(Common.getClientSessionData(), emp);
        //                wLogger.Info("InsUpdClientDetails response" + JsonConvert.SerializeObject(insertReply));

        //                if (insertReply.ReplyData.ReplyCode == 0)
        //                {
        //                    //*********************send activation eamil *******************
        //                    LoanAPI.TReplyData activationReply = new LoanAPI.TReplyData();
        //                    return RedirectToAction("Index", new
        //                    {
        //                        strErrorMsg = "SP_Profile updated successfully"
        //                    });
        //                }
        //                else
        //                {
        //                    //*****************display error message to the user**************
        //                    return RedirectToAction("Index", new
        //                    {
        //                        strErrorMsg = "Technical problem," + insertReply.ReplyData.ReplyMessage
        //                    });
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", new
        //            {
        //                strErrorMsg = "Invalid Session"
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errorLogger.Info("ProfileController=>UpdateProfile " + ex.Message + " " + ex.StackTrace);
        //    }
        //    return RedirectToAction("Index", new
        //    {
        //        strErrorMsg = "Error occured during update"
        //    });
        //}























        public void SendUpdateDetailsEmail(int iClientID, string strEmailId, string activationGUID, string strUserName, string emailAddress, string mobileNumber, string originalMobile)
        {
            StringBuilder sb = new StringBuilder("");

            DateTime currentTime = DateTime.Now;
            DateTime bufferTime = currentTime.AddMinutes(10);

            string submissionDate = bufferTime.ToString("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss", CultureInfo.InvariantCulture);
            string encrSubDate = Common.Encrypt(submissionDate, true);
            string encoSubDate = HttpUtility.UrlEncode(encrSubDate, Encoding.ASCII);

            string encrEmail = Common.Encrypt(emailAddress, true);
            string encoEmail = HttpUtility.UrlEncode(encrEmail, Encoding.ASCII);

            string encrMobile = Common.Encrypt(mobileNumber, true);
            string encoMobile = HttpUtility.UrlEncode(encrMobile, Encoding.ASCII);

            string encrOMobile = Common.Encrypt(originalMobile, true);
            string encoOMobile = HttpUtility.UrlEncode(encrOMobile, Encoding.ASCII);

            string idNumber = Common.LoggedInClientIDNumber;

            string encrID = Common.Encrypt(idNumber, true);
            string encoID = HttpUtility.UrlEncode(encrID, Encoding.ASCII);

            string strURL = "";

            strURL = ConfigurationManager.AppSettings["UpdateDetailsURL"].ToString() + "?CID=" + iClientID + "&LinkID=" + activationGUID + "&sm=" + encoSubDate + "&e=" + encoEmail + "&c=" + encoMobile + "&i=" + encoID + "&om=" + encoOMobile + "";
            
            sb.Append("<map name='mapFooLink'>");
            sb.Append("	<area shape='rect' coords='133,21,283,62' href='http://www.net1.com/' />");
            sb.Append("</map>");
            sb.Append("<table width=\"960px\" cellpadding=\"0\" cellspacing=\"0\" style=\"color:#9EA4A9;\">");
            sb.Append("<tr>");
            sb.Append("<td ><img  usemap=\"#mapFooLink\" src=\"cid:imgHEADER\" width=\"960\" height=\"216\" alt=\"FINBOND\" /></td>");
            sb.Append("</tr>");
            sb.Append("<tr><td style =\"font-family:Futura Lt BT;color:#4a4a4a\">");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:13pt;color:#9EA4A9\">Dear " + strUserName + ",</div>");
            sb.Append("<br />");
            sb.Append("<div  style=\"font-family:arial;font-size:10pt;color:#9EA4A9;\">Please click on below URL or Alternatively, you can copy this link and paste it into your browser to update your account:</ div>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append(strURL);
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-weight:bold;font-size:10pt;color:#003F7D; display:inline-block\">If you would like to find out more about Finbond Credit, please refer to our &nbsp;</div>");
            sb.Append("<a style=\"display:inline-block\" href=\"https://www.finbondcredit.co.za/faq \"> [FAQ]  </a>");
            sb.Append("<div style=\"font-family:arial;font-weight:bold;font-size:10pt;color:#003F7D; display:inline-block\"> &nbsp; or contact us by phone or email.</div>");
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
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">If you did not apply for this services please ignore this or email:loans@finbond.co.za  or contact our call center: 086 000 4249 to log a case?</div>");
            sb.Append("<br />");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td ><img  src=\"cid:imgFOOTER\" width=\"960\" height=\"489\" alt=\"FINBOND\" /></td>");
            sb.Append("</tr>");
            sb.Append("</table>");

            List<string> lst = new List<string>();
            lst.Add(strEmailId);
            Emails.SendEmail(sb, "Finbond Loan Account Update", lst);
            wLogger.Info("Finbond Loan Account Update:  " + lst + sb.ToString());
        }

    }
}