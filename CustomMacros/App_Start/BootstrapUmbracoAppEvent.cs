using System;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CustomMacros.Areas.Infrastructure.Injection.MVC;
using Umbraco.Core;

namespace CustomMacros.App_Start
{
    /// <summary>
    /// Custom management of Umbraco Application Events. Register classes into the DI Container
    /// </summary>
    public class BootstrapUmbracoAppEvent : ApplicationEventHandler
    {
        private static IWindsorContainer container;

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            // see: http://our.umbraco.org/documentation/reference/Templating/Mvc/using-ioc
            BootstrapDIContainer();
            // DI Container's Dispose at the end of the Umbraco Application
            umbracoApplication.Disposed += new EventHandler((object sender, EventArgs e) => { container.Dispose(); });
        }

        /// <summary>
        /// DI Container Bootstrap and install of Windsor Installer
        /// </summary>
        private static void BootstrapDIContainer()
        {
            container = new WindsorContainer()
                .Install(
                    FromAssembly.InDirectory(new AssemblyFilter(AssemblyDirectory))
                    );

            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            // Using WindsorControllerFactory rather then DependencyResolver
               // http://bradwilson.typepad.com/blog/2010/07/service-location-pt1-introduction.html (read comments)
               // http://bradwilson.typepad.com/blog/2010/10/service-location-pt5-idependencyresolver.html (read comments)
               // http://bradwilson.typepad.com/blog/2010/10/service-location-pt10-controller-activator.html (read comments)
               // http://mikehadlow.blogspot.com/2011/02/mvc-30-idependencyresolver-interface-is.html
               // http://kozmic.pl/2010/08/19/must-windsor-track-my-components/
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);        
        }


        /// <summary>
        /// Local Directory where are present all the assemblies
        /// </summary>
        static public string AssemblyDirectory
        {
            //Snippet code from: https://gist.github.com/iamkoch/2344638
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}