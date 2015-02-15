using Castle.MicroKernel.Registration;
using CustomMacros.Areas.Sample.Services;

namespace CustomMacros.Areas.Sample.Injection.Installers
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<IDataBase>().ImplementedBy<DataBase>().LifeStyle.Singleton);
        }
    }
}