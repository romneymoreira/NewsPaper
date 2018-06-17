using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjetoJornal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "View",
            //    url: "View/{id}",
            //    defaults: new { controller = "Home", action = "View", id = UrlParameter.Optional }
            //    , namespaces: new[] { "ProjetoJornal.Controllers" }
            //); //Route: /View/12

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , namespaces: new[] { "ProjetoJornal.Controllers" }
            );
        }
    }
}
