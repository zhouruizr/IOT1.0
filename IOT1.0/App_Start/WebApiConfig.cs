using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace IOT1._0
{
    public static class WebApiConfig
    {
        public static void Register(RouteCollection routes)
        {
            var route = routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "ERP", action = "Get", id = UrlParameter.Optional }
            );
            route.RouteHandler = new WebApiSessionRouteHandler();

        }
    }
}
