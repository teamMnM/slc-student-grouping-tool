using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace TeamMnMGroupingWebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //unregister Web Forms view engine so that the view locator doesn't search for view locations for Web Forms
            var viewEngines = System.Web.Mvc.ViewEngines.Engines;
            var webformsEngine = viewEngines.OfType<WebFormViewEngine>().FirstOrDefault();
            if (webformsEngine != null)
                viewEngines.Remove(webformsEngine);
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

            //Ensure SessionID in order to prevent the folloing exception
            //when the Application Pool Recycles
            //[HttpException]: Session state has created a session id, but cannot
            //    save it because the response was already flushed by 
            string id = Session.SessionID;
        }

        protected void Application_OnPostAuthenticateRequest(Object sender, EventArgs e)
        {
            IPrincipal contextUser = Context.User;

            if (contextUser.Identity.AuthenticationType == "Forms")
            {
                // determine role name
                string[] roles = new string[1];
                roles[0] = "user";

                // attach to context
                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(User.Identity, roles);
                Thread.CurrentPrincipal = HttpContext.Current.User;
            }
        }
    }
}