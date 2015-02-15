using System.Diagnostics;
using System.Web.Mvc;

namespace eice.framework.Areas.Infrastructure.Filters
{
    public class ProfileAttribute :ActionFilterAttribute
    {
        private Stopwatch timer;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            timer = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Debug.WriteLine(string.Format("Profile - OnActionExecuted time: {0}", timer.Elapsed.TotalMilliseconds));
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Debug.WriteLine(string.Format("Profile - OnResultExecuting time: {0}", timer.Elapsed.TotalMilliseconds));
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            timer.Stop();
            Debug.WriteLine(string.Format("Profile - OnResultExecuted time: {0}", timer.Elapsed.TotalMilliseconds));
        }
    }
}