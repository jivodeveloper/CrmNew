using Call_Centre_Management.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Call_Centre_Management
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            new SchedulerTask().Start(18,34);
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            try
            {
                //new SlackClient().sendMessage(ex.Message, ex.StackTrace, SlackColor.danger);
            }
            catch (Exception)
            {
            }
            //new TeamsClient().sendMessage(ex.Message, ex.StackTrace, TeamsColor.danger);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.Flush();
            }
        }
    }
}
