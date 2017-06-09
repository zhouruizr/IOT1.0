using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace IOT1._0
{
    public class WebApiSessionControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public WebApiSessionControllerHandler(RouteData routeData) : base(routeData) { }
    }

    public class WebApiSessionRouteHandler : IRouteHandler//HttpControllerRouteHandler
    {
        IHttpHandler IRouteHandler.GetHttpHandler(RequestContext requestContext)
        {
            return new WebApiSessionControllerHandler(requestContext.RouteData);
        }
    }
}