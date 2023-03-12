using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using StaffLoans.LoanAPI;
using StaffLoans.Controllers;
using Newtonsoft.Json;
using log4net;

namespace StaffLoans.Models
{
    public class UpdateOTP
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");

        [Required(ErrorMessage = " ")]
        [Display(Name = "Please input the OTP below")]
        public string CellNumberOTPPrompt { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string CellNumber { get; set; }

        public string OTP { get; set; }

        public string IDNumber { get; set; }

        public string NewEmail { get; set; }

        public string NewCell { get; set; }

        public uint ClientCode { get; set; }

        public string UpdateID { get; set; }

        public TReplyData replyData { get; set; }

        public static UpdateOTP ValidateUpdateLink(uint clientCode, string updateID)
        {
            using (var proxy = new LoanAPI.FINBONDAPIClient())
            {
                //wLogger.Info("OperatorLogon request");
                //LoanAPI.TLogonReply loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                //wLogger.Info("OperatorLogon response" + JsonConvert.SerializeObject(loginReply));
                //if (loginReply.ReplyData.ReplyCode == 0)
                //{
                //Common.OperatorLoginSessionId = loginReply.SessionData.SessionID;
                wLogger.Info("ValidateUpdateLink request");
                TClientValidateActivationReply response = proxy.ValidateActivationLink(Common.getOperatorSessionData(), clientCode, updateID);
                wLogger.Info("ValidateUpdateLink response" + JsonConvert.SerializeObject(response));
                return new UpdateOTP()
                {
                    FirstName = response.FirstName,
                    Surname = response.LastName,
                    replyData = response.ReplyData,
                    ClientCode = clientCode,
                    UpdateID = updateID,
                    IDNumber = response.IDNumber
                };
                //}
            }

            //return null;
        }
    }
}