using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebNails.Transactions
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                "Login", "login.html",
                new
                {
                    controller = "Login",
                    action = "Index"
                });

            routes.MapRoute(
                "Logout", "logout.html",
                new
                {
                    controller = "Login",
                    action = "Logout"
                });

            routes.MapRoute(
                "Transactions", "transactions.html",
                new
                {
                    controller = "Home",
                    action = "Index"
                });
        }
    }
}
