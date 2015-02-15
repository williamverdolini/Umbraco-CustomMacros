using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CustomMacros.Areas.Infrastructure.Commons;
using CustomMacros.Areas.Infrastructure.Models.Exceptions;

namespace CustomMacros.Areas.Infrastructure.Filters
{
    public class SessionExpiredExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public string RedirectTo {get; set;}

        public SessionExpiredExceptionAttribute()
        {
            RedirectTo = "/";
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception is SessionExpiredException)
            {
                FormsAuthentication.SignOut();
                filterContext.HttpContext.Session.Abandon();
                filterContext.HttpContext.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                string strOutput = Utilities.RedirectClientJson(RedirectTo);
                filterContext.HttpContext.Response.Write(strOutput);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}