using System.Web.Mvc;
using CustomMacros.Areas.Infrastructure.Commons;

namespace CustomMacros.Areas.Infrastructure.Filters
{
    public class LogExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public string RedirectTo {get; set;}

        public LogExceptionAttribute()
        {
            RedirectTo = "/";
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string strOutput = Utilities.RedirectClientJson(RedirectTo);
                filterContext.HttpContext.Response.Write(strOutput);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}