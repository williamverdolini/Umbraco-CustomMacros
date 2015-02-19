using System;
using Castle.MicroKernel.Registration;
using CustomMacros.Areas.Infrastructure.Commons;
using CustomMacros.Areas.Infrastructure.Services.PopulateData;

namespace CustomMacros.Areas.Infrastructure.Injection
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");
            container.Register(Component.For<IPopulateInitialData>().ImplementedBy<PopulateInitialData>().LifeStyle.Singleton);
        }
    }
}