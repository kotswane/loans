using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffLoans.Controllers
{
    public class WelcomeController : BaseController
    {
        [CustomAuthorize]
        public ActionResult Index(string strMsg)
        {
            if (!string.IsNullOrEmpty(strMsg))
                ShowMessages(strMsg);
            return View();
        }
    }
}