using System;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CustomMacros.App_Start;
using CustomMacros.Areas.Infrastructure.Commons;
using CustomMacros.Areas.Infrastructure.Controllers;
using CustomMacros.Areas.Infrastructure.Injection.MVC;

namespace CustomMacros.Areas.Infrastructure.Injection.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");

            // Register MVC ActionInvoker
            container.Register(Component.For<IActionInvoker>().ImplementedBy<WindsorActionInvoker>().LifeStyle.Transient);

            // Register the MVC Controllers
            container.Register(
                Classes
                .FromAssemblyInDirectory(new AssemblyFilter(BootstrapUmbracoAppEvent.AssemblyDirectory))
                .BasedOn<IController>()
                .LifestyleTransient());

            // Register Controller's Workers
            container.Register(
                Classes
                .FromAssemblyInDirectory(new AssemblyFilter(BootstrapUmbracoAppEvent.AssemblyDirectory))
                .BasedOn<IWorker>()
                .WithService.FromInterface()
                .LifestyleTransient()
                );

        }
    }
}