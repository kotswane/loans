using StaffLoans.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace StaffLoans.Controllers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
     //   Entities context = new Entities(); // my entity  
        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute()
        {
//            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            if (Common.ClientLoginSessionId  != "" && Common.ClientLoginSessionId != null)
                authorize=true;

            //if (HttpContext.Current.Session["AuthToken"] != null || HttpContext.Current.Request.Cookies["AuthToken"] != null)
            //{
            //    if (!HttpContext.Current.Session["AuthToken"].ToString().Equals(
            //       HttpContext.Current.Request.Cookies["AuthToken"].Value))
            //        authorize = false;
            //}
            //foreach (var role in allowedroles)
            //{
            //    var user = context.AppUser.Where(m => m.UserID == GetUser.CurrentUser/* getting user form current context */ && m.Role == role &&
            //    m.IsActive == true); // checking active users with allowed roles.  
            //    if (user.Count() > 0)
            //    {
            //        authorize = true; /* return true if Entity has current user(active) with specific role */
            //    }
            //}
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new HttpUnauthorizedResult();
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
        }
    }  
}
