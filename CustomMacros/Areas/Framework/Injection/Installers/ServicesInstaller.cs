using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CustomMacros.Areas.Framework.Services.Application;
using CustomMacros.Areas.Framework.Services.Page;

namespace eice.framework.Areas.Framework.Injection.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IPageService>().ImplementedBy<PageService>().LifeStyle.Singleton);
            container.Register(Component.For<ISessionService>().ImplementedBy<SessionService>().LifeStyle.Singleton);
            container.Register(Component.For<IRequestService>().ImplementedBy<RequestService>().LifeStyle.Singleton);

        }
    }
}