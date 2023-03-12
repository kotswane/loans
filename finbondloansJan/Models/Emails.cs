using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using log4net;

namespace StaffLoans.Models
{
    public class Emails
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");

        static string FromAddress = ConfigurationManager.AppSettings["FromAddress"];
        static string FromName = ConfigurationManager.AppSettings["MailName"];
        //static string EmailHost = ConfigurationManager.AppSettings["EmailHost"];
        //static string EmailUsername = ConfigurationManager.AppSettings["EmailUsername"];
        //static string EmailPassword = ConfigurationManager.AppSettings["EmailPassword"];


        public static bool SendEmail(StringBuilder sbAgData, string strSubject, List<string> toMailIdList)
        {
            try
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                msg.IsBodyHtml = true;

                //**********************start for embed images *****************************
                string strImgPath = "";
                //if (blnActivationMail)
                //{
                //    strImgPath = Server.MapPath("images\\emailIMAGES\\header_1.gif");
                //}
                //else
                //{
                    strImgPath = HttpContext.Current.Server.MapPath("~\\Content\\images\\emailIMAGES\\Header1.png");
                //}
                System.Net.Mail.LinkedResource img1 = new System.Net.Mail.LinkedResource(strImgPath);
                img1.ContentId = "imgHEADER";
                System.Net.Mail.AlternateView av1 = System.Net.Mail.AlternateView.CreateAlternateViewFromString(sbAgData.ToString(), null, System.Net.Mime.MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(img1);

                string strFooterImgPath = HttpContext.Current.Server.MapPath("~\\Content\\images\\emailIMAGES\\FooterImg.png");
                System.Net.Mail.LinkedResource imgFoot = new System.Net.Mail.LinkedResource(strFooterImgPath);
                imgFoot.ContentId = "imgFOOTER";

                av1.LinkedResources.Add(imgFoot);
                msg.AlternateViews.Add(av1);

                msg.From = new System.Net.Mail.MailAddress(FromAddress, FromName);
                msg.Sender = new System.Net.Mail.MailAddress(FromAddress, FromName);
                for (int i = 0; i < toMailIdList.Count; i++)
                {
                    msg.To.Add(toMailIdList[i].ToString());
                }

                msg.Subject = strSubject;
                msg.Priority = System.Net.Mail.MailPriority.Normal;


                //string strImgPath = "";
                //string strSubHeader = "";

                //System.Net.Mail.AlternateView htmlView = null;
                //htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(sbAgData.ToString(), null,
                //                                                                       System.Net.Mime.
                //                                                                           MediaTypeNames.Text.Html);

                //msg.AlternateViews.Add(htmlView);
                msg.IsBodyHtml = true;

                MailData MM = new MailData(msg, 1);
                MailingThread Mthcls = new MailingThread(MM);
                System.Threading.Thread Mth = new System.Threading.Thread(new System.Threading.ThreadStart(Mthcls.SenMail));
                Mth.Start();

                //EmailLog.InsertLogEntry(
                //        new MoneyLineLib.Models.EmailLog()
                //        {
                //            Message = sbAgData.ToString(),
                //            Subject = strSubject,
                //            To = string.Join(";", toMailIdList.ToArray())
                //        }
                //    );
                wLogger.Info("Email Sent Successfully " + sbAgData + strSubject + toMailIdList.FirstOrDefault());
                return true;
            }
            catch (Exception ex)
            {
                wLogger.Info("Email Error "+ ex.Message + " "  + sbAgData + strSubject + toMailIdList.FirstOrDefault());
                //MessageWrapper.SendErrorEmail(ex, "Error");
                return false;
                throw;
            }

            //return false;
        }


        public class MailData
        {
            public System.Net.Mail.MailMessage _m = null;
            public Int32 _pk = 0;
            public MailData(System.Net.Mail.MailMessage M, Int32 pk)
            {
                _m = M;
                _pk = pk;
            }

        }
        public class MailingThread
        {
            private System.Net.Mail.MailMessage _m = null;
            private Int32 _pk;
            public MailingThread(MailData Mail)
            {
                _m = Mail._m;
                _pk = Mail._pk;
            }
            public void SenMail()
            {
                try
                {
                    System.Net.Mail.SmtpClient PostOffice = new System.Net.Mail.SmtpClient();
                    //System.Net.NetworkCredential cred = new System.Net.NetworkCredential(EmailUsername, EmailPassword);

                    //PostOffice.Credentials = cred;
                    //PostOffice.Host = EmailHost;
                    PostOffice.Host = "smtp.net1.com";
                    PostOffice.Send(_m);
                }
                catch (Exception ex)
                {

                    // Log.Error("Service->SenMail()", ex.Message, ex.StackTrace);
                    throw;
                }

            }
        }
    }
}