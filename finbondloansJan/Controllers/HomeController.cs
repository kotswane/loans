using log4net;
using StaffLoans.LoanAPI;
using StaffLoans.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace StaffLoans.Controllers
{
    public class HomeController : BaseController
    {
        private static readonly ILog wLogger = LogManager.GetLogger("CallMeBackRequestsLogger");

        public ActionResult Index(string strMessage)
        {
            //FinService.FinServiceClient client = new FinService.FinServiceClient();
            //client.SendSMS("0725973216", "Good Day" + Environment.NewLine + "Your requested OTP is " , "FinLoan");
            //client.Close();

            if (!string.IsNullOrEmpty(strMessage))
                ShowMessages(strMessage);
           // TestEmail("Laxmi", "http://localhost:52141/");
            //string Path = @"C:\Users\laxmi\Documents\Projects\Projects\FinbondLoans\Finbond_Staffloans_Main\FinbondLoans\Content\Images\FINBOND LOGO.png";
            //using (Image image = Image.FromFile(Path))
            //{
            //    using (MemoryStream m = new MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();

            //        // Convert byte[] to Base64 String
            //        string base64String = Convert.ToBase64String(imageBytes);
            //        var test = base64String;

            //    }
            //}

            //**********************chk if cookie exists or not*****************
            if(System.Web.HttpContext.Current.Request.Cookies["FinbondCellc"] == null)
            {
                //**********************call api and create cookie
                using (var proxy = new LoanAPI.FINBONDAPIClient())
                {
                    TReplyData reply = new TReplyData();
                    reply = proxy.RecordVisit(Convert.ToUInt32(ConfigurationManager.AppSettings["Origin"].ToString()));
                    if (reply.ReplyCode == 0)
                    {
                        createCookie();
                    }
                }
            }
            return View();
        }

        private void TestEmail(string strUserName, string strURL)
        {
            StringBuilder sb = new StringBuilder("");
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
            sb.Append("<div  style=\"font-family:arial;font-size:10pt;color:#9EA4A9;\">Please click on below button or Alternatively, you can copy this link and paste it into your browser to activate your account:</ div>");
            sb.Append("<br />");
            sb.Append(strURL);
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-weight:bold;font-size:10pt;color:#003F7D\">FINBOND LOANS TEAM</div>");
            sb.Append("<br />");
            sb.Append("<div style=\"font-family:arial;font-size:10pt;color:#9EA4A9\">If you did not apply for this services please ignore this or email:info@finbondcredit.com or contact our call center: 011 234 5678 to log a case?</div>");
            sb.Append("<br />");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td ><img  src=\"cid:imgFOOTER\" width=\"960\" height=\"489\" alt=\"FINBOND\" /></td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            List<string> lst = new List<string>();
            lst.Add("laxmi.guntuka@net1.com");
            Emails.SendEmail(sb, "Test Staff Loan", lst);
        }

        [HttpPost]
        public JsonResult SaveCallMeBack(string strTitle, string strFullName,string strPhone)
        {
            wLogger.Info("Call Me Back Request: Title|" + strTitle + ",Name|" + strFullName + ",Mobile|" + strPhone + "");
            return Json("Success");
        }

        private bool createCookie()
        {
            Boolean blnSuccess = false;
            try
            {
                //HttpContext.Current.Request.Cookies.Remove("MarkitLogin");
                if (System.Web.HttpContext.Current.Request.Cookies["FinbondCellc"] != null)
                {

                    HttpCookie LoansCookie = System.Web.HttpContext.Current.Request.Cookies["FinbondCellc"];
                    LoansCookie.Expires = DateTime.Now.AddDays(-1);
                    System.Web.HttpContext.Current.Response.Cookies.Add(LoansCookie);
                }
                //if (HttpContext.Current.Request.Cookies["MarkitLogin"] == null)
                //{
                HttpCookie Loanscookie = new HttpCookie("FinbondCellc");
                Loanscookie["FromFinbond"] = "true";
                //Loanscookie["BranchID"] = iBranchId.ToString();
                //Loanscookie["BranchName"] = strBranchName;

                Loanscookie.Expires = DateTime.Now.AddDays(365);
                System.Web.HttpContext.Current.Response.Cookies.Add(Loanscookie);


                blnSuccess = true;
            }
            catch (Exception ex)
            {
                //Log.Error("Auth.cs->CreateLoginCookie()", ex.Message, ex.StackTrace);
            }
            return blnSuccess;
        }
    }
}