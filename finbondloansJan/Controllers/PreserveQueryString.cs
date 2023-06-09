﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StaffLoans.Controllers
{
    public class PreserveQueryStringAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var redirectResult = filterContext.Result as RedirectToRouteResult;
            if (redirectResult == null)
            {
                return;
            }

            var query = filterContext.HttpContext.Request.QueryString;
            // Remark: here you could decide if you want to propagate all
            // query string values or a particular one. In my example I am
            // propagating all query string values that are not already part of
            // the route values
            foreach (string key in query.Keys)
            {
                if (!redirectResult.RouteValues.ContainsKey(key))
                {
                    redirectResult.RouteValues.Add(key, query[key]);
                }
            }
        }
    }
}