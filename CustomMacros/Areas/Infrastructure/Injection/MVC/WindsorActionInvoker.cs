using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;
using Umbraco.Web.Mvc;

namespace CustomMacros.Areas.Infrastructure.Injection.MVC
{
    //thanks to: http://stackoverflow.com/questions/6622613/castle-windsor-injecting-iactioninvoker-implementation-issue
    //thanks to: http://weblogs.asp.net/psteele/using-windsor-to-inject-dependencies-into-asp-net-mvc-actionfilters
    public class WindsorActionInvoker : RenderActionInvoker 
    {
        readonly IKernel kernel;
        public WindsorActionInvoker(IKernel kernel)
        {
            this.kernel = kernel;
        }

        protected override IAsyncResult BeginInvokeActionMethodWithFilters(ControllerContext controllerContext, IList<IActionFilter> filters, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters, AsyncCallback callback, object state)
        {
            foreach (IActionFilter actionFilter in filters)
            {
                //Inject Properties in all the filters but global filters (already injected by Windsor's Controller Factory)
                if (!typeof(Infrastructure.Controllers.MacroController).IsAssignableFrom(actionFilter.GetType()))
                    kernel.InjectProperties(actionFilter);
            }
            return base.BeginInvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters, callback, state);
        }
    }


    public static class WindsorExtension
    {
        public static void InjectProperties(this IKernel kernel, object target)
        {
            var type = target.GetType();
            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.CanWrite && kernel.HasComponent(property.PropertyType))
                {
                    var value = kernel.Resolve(property.PropertyType);
                    try
                    {
                        property.SetValue(target, value, null);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("Error setting property {0} on type {1}, See inner exception for more information.", property.Name, type.FullName);
                        throw new ComponentActivatorException(message, ex, null);
                    }
                }
            }
        }
    }
}