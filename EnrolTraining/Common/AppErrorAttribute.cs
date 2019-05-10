using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnrolTraining.Common
{
    public class AppErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                String err = "Sorry, your request could not be processed due to some eror. contact your service provider.";
                filterContext.HttpContext.Response.StatusCode = 500;
                bool isRedirectError = String.IsNullOrEmpty(filterContext.HttpContext.Request.Headers["redirectOnError"]) ? false : Convert.ToBoolean(filterContext.HttpContext.Request.Headers["redirectOnError"]);

                if (isRedirectError)
                {
                    filterContext.Result = new JsonResult { Data = new { result = "Redirect", url = "/App/InternalServerError" } };
                }
                else
                {
                    filterContext.Result = new JsonResult { Data = err };
                }
                filterContext.ExceptionHandled = true;
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "App", action = "Error" }));
            }
        }
    }
}