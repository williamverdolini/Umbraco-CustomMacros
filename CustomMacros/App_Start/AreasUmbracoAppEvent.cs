using System;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;

namespace CustomMacros.App_Start
{
    // Official Documentation: http://our.umbraco.org/documentation/Reference/Events/application-startup#UsingApplicationEventHandlertoregisterevents
    public class AreasUmbracoAppEvent : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            AreaRegistration.RegisterAllAreas();
        }

        /// <summary>
        /// Bind to the events of the HttpApplication
        /// </summary>
        void UmbracoApplicationBase_ApplicationOnError(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            app.Context.Response.Redirect("error");
        }
    }
}