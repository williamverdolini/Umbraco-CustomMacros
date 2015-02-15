using System.Web.Mvc;
using CustomMacros.Areas.Framework.Services.Page;
using CustomMacros.Areas.Infrastructure.Controllers;

namespace CustomMacros.Areas.Infrastructure.Filters
{
    public class MapToViewAttribute : ActionFilterAttribute
    {
        public string ViewName { get; set; }

        private const string INIT_VIEW_NAME = "Init";

        public MapToViewAttribute()
        {
            ViewName = INIT_VIEW_NAME;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Result is PartialViewResult)
            {
                if (ViewExists(filterContext.Controller.ControllerContext, ViewName))
                    (filterContext.Result as PartialViewResult).ViewName = ViewName;
            }
        }

        private bool ViewExists(ControllerContext controllerContext, string name)
        {
            ViewEngineResult result = ViewEngines.Engines.FindView(controllerContext, name, null);
            return (result.View != null);
        }

    }
}