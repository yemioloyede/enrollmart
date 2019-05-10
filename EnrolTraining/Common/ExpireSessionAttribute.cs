using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;

namespace EnrolTraining.Common
{
    public class ExpireSessionAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["LoginUser"] == null)
            {
                FlashMessage.Danger("Your Session has been expired");
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Access", action = "Login", returnUrl = filterContext.HttpContext.Request.RawUrl }));
            }
        }
    }
}