using System.Configuration;

namespace CustomMacros.Areas.Infrastructure.Services
{
    public interface IInitializationService
    {
        void Initialize(ConnectionStringSettingsCollection connections);
    }
}
