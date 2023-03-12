using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;

namespace StaffLoans
{
    public class TwoFactorProfile /*: ProfileBase*/
    {
        public static TwoFactorProfile CurrentUser
        {
            get
            {
                //bh return GetByUserName(Membership.GetUser().UserName);
                return new TwoFactorProfile()
                {
                    //TwoFactorSecret = "wlIn959uC7"
                };
            }
        }

        //public static TwoFactorProfile GetByUserName(string username)
        //{
        //    return (TwoFactorProfile)Create(username);
        //}

        public DateTime? LastLoginAttemptUtc
        {
            get
            {
                try
                {
                    //bh return (DateTime?)base["LastLoginAttemptUtc"];
                    return DateTime.Now;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                //bh base["LastLoginAttemptUtc"] = value;
                //bh Save();
            }
        }

        public string TwoFactorSecret
        {
            get
            {
                //bh return (string)base["TwoFactorSecret"];
                return "wlIn959uC7";
            }
            set
            {
                //bh base["TwoFactorSecret"] = value;
                //bh Save();
            }
        }
    }
}