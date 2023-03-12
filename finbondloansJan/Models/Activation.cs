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
    public class Activation
    {
        private static readonly ILog wLogger = LogManager.GetLogger("InfoLogs");

        public string EncodedSecret { get; set; }

        [Required(ErrorMessage = " ")]
        [Display(Name = "Please capture relevant code from the Google Authenticator application")]
        public string ConfirmTwoFactorCode { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string EmailAddress { get; set; }

        public uint ClientCode { get; set; }

        public string ActivationID { get; set; }

        public TReplyData replyData { get; set; }

        public static Activation ValidateActivationLink(uint clientCode, string activationID)
        {
            using (var proxy = new LoanAPI.FINBONDAPIClient())
            {
                //wLogger.Info("OperatorLogon request");
                //LoanAPI.TLogonReply loginReply = proxy.OperatorLogon(ConfigurationManager.AppSettings["OperatorLoginIDnumber"], 1);
                //wLogger.Info("OperatorLogon response" + JsonConvert.SerializeObject(loginReply));
                //if (loginReply.ReplyData.ReplyCode == 0)
                //{
                    //Common.OperatorLoginSessionId = loginReply.SessionData.SessionID;
                    wLogger.Info("ValidateActivationLink request");
                    TClientValidateActivationReply response = proxy.ValidateActivationLink(Common.getOperatorSessionData(), clientCode, activationID);
                    wLogger.Info("ValidateActivationLink response" + JsonConvert.SerializeObject(response));
                    return new Activation()
                    {
                        FirstName = response.FirstName,
                        Surname = response.LastName,
                        replyData = response.ReplyData,
                        ClientCode = clientCode,
                        ActivationID = activationID
                    };
                //}
            }

            //return null;
        }

        public static TReplyData SetClientActivation(uint clientCode, string twoFASecret, string lastLoginAttemptUCA)
        {
            using (var proxy = new LoanAPI.FINBONDAPIClient())
            {
                lastLoginAttemptUCA = null;
                wLogger.Info("ClientLoginAndSecretUpdate request");
                TReplyData response = proxy.ClientLoginAndSecretUpdate(Common.getOperatorSessionData(), clientCode, twoFASecret, lastLoginAttemptUCA);
                wLogger.Info("ClientLoginAndSecretUpdate response" + JsonConvert.SerializeObject(response));
                return response;
            }
        }
    }
}