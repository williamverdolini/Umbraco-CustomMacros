
namespace CustomMacros.Areas.Framework.Services.Application
{
    public interface ISessionService
    {
        bool Contains(string key);
        T Get<T>(string key);
        void Remove(string key);
        void Set<T>(string key, T value);
        void Abandon();
    }
}
