using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffLoans.Controllers
{
    public class BaseController : Controller
    {
        public void ShowMessages(string strResourceValue)
        {
            if (strResourceValue != null && strResourceValue != "")
            {
                string strDispalyType = "SE";
                string strMessage = "";
                string strPopupDescription = "";
                if (strResourceValue.Contains('_'))
                {
                    var arr = strResourceValue.Split('_');

                    if (arr.Length > 0)
                    {
                        strDispalyType = arr[0];
                        strMessage = arr[1];
                        if (arr.Length > 2)
                            strPopupDescription = arr[2];
                    }
                }
                else
                {
                    strMessage = strResourceValue;
                }


                switch (strDispalyType)
                {
                    case "SE":
                        //if (blnRegMessage)
                        //{
                        //    ViewBag.ShowRegErrorMessage = "True";
                        //    ViewBag.ShowRegErrorMessageDescription = strMessage;
                        //}
                        //else
                        //{
                        ViewBag.ShowErrorMessage = "True";
                        ViewBag.ShowErrorMessageDescription = strMessage;
                        //}
                        break;
                    case "SI":
                        ViewBag.ShowInfoMessage = "True";
                        ViewBag.ShowInfoMessageDescription = strMessage;
                        break;
                    case "SP":
                        ViewBag.ShowSuccessMessage = "True";
                        ViewBag.ShowSuccessMessageDescription = strMessage;
                        break;
                    case "CE":
                        ViewBag.ShowContextErrorMessage = "True";
                        ViewBag.ShowContextErrorMessageDescription = strMessage;
                        break;
                    case "CI":
                        ViewBag.ShowContextInfoMessage = "True";
                        ViewBag.ShowContextInfoMessageDescription = strMessage;
                        break;

                    case "CP":
                        ViewBag.ShowContextSuccessMessage = "True";
                        ViewBag.ShowContextSuccessMessageDescription = strMessage;
                        break;
                    case "POPUP":
                        ViewBag.ShowStaticPopupMessage = "True";
                        ViewBag.ShowStaticPopupMessageTitle = strMessage;
                        ViewBag.ShowStaticPopupMessageDescription = strPopupDescription;
                        break;
                }

            }
        }
    }
}