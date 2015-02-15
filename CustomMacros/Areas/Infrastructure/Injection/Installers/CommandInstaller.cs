using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CustomMacros.Areas.Infrastructure.Commands;
using CustomMacros.Areas.Infrastructure.Commons;

namespace CustomMacros.Areas.Infrastructure.Injection.Installers
{
    public class CommandInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");
            container.Register(Component.For<ICommandFactory>().ImplementedBy<CommandFactory>().LifeStyle.Transient);
        }
    }
}