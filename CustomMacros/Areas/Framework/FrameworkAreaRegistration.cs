using System.Web.Mvc;

namespace CustomMacros.Areas.Framework
{
    public class FrameworkAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Framework";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name : "Framework_default",
                url: "Framework/{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
