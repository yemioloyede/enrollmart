using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EnrolTraining
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("DomainRoute", new Routing.DomainRoute(
                "{company}.enrollmart.com", //domain for production e.g. enrollmart.com, localhost for development
                "{action}/{id}",
                new { controller = "Enroll", action = "Calendar", id = UrlParameter.Optional }
            ));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Access", action = "Login", id = UrlParameter.Optional },
                constraints: new { Controller = "^Access$|^Admin$|^Filemanager$" }
            );

        }
    }
}
