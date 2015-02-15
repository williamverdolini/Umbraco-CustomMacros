using System.Web;

namespace CustomMacros.Areas.Framework.Services.Application
{
    public class SessionService : ISessionService
    {
        public bool Contains(string key)
        {
            if (HttpContext.Current.Session != null)
                return (HttpContext.Current.Session[key] != null);
            else
                return false;
        }

        public void Remove(string key)
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session.Remove(key);
        }

        public T Get<T>(string key)
        {
            if (HttpContext.Current.Session != null)
                return (T)HttpContext.Current.Session[key];
            else
                return default(T);
        }

        public void Set<T>(string key, T value)
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session[key] = value;
        }

        public void Abandon(){
            // rif.: https://support.microsoft.com/kb/899918/it?wa=wsignin1.0
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
        }

    }
}