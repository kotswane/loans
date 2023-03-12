using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffLoans.Controllers
{
    public class CustomErrorsController :BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}