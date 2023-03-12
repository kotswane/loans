using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffLoans.Controllers
{
    public class LoanCompletionController : BaseController
    {
        [CustomAuthorize]
        public ActionResult Index()
        {
           // TempData["ApplicationNumber"] = "43545";
            return View();
        }
    }
}