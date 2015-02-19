using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CustomMacros.Areas.Infrastructure.Services
{
    public class CustomMacrosSetup
    {
        private static CustomMacrosSetup instance;
        private IList<IInitializationService> appServices;

        private CustomMacrosSetup()
        {
            appServices = new List<IInitializationService>();
        }

        public static CustomMacrosSetup Create(){
            return instance ?? (instance = new CustomMacrosSetup());
        }

        public CustomMacrosSetup RegisterInizializationService(IInitializationService service)
        {
            if(!appServices.Contains(service))
                appServices.Add(service);
            return this;
        }

        public void InitializeServices()
        {
            appServices.ToList<IInitializationService>().ForEach(service => service.Initialize(ConfigurationManager.ConnectionStrings));
        }
    }
}