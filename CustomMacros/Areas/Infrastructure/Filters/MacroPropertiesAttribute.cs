using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using CustomMacros.Areas.Framework.Services.Application;
using CustomMacros.Areas.Infrastructure.Attributes;
using CustomMacros.Areas.Infrastructure.Controllers;

namespace CustomMacros.Areas.Infrastructure.Filters
{
    public class RetrieveMacroPropertiesAttribute : ActionFilterAttribute
    {
        public ISessionService SessionService { get; set; }
        private MacroController controller;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            controller = filterContext.Controller as MacroController;
            GetMacroProperties();
        }

        private void GetMacroProperties()
	    {
	        var props = GetMacroProperties(controller.GetType());	
	        foreach (PropertyInfo prop in props)
	            prop.SetValue(controller, GetModuleParam(prop.Name), null);
	    }

        private List<PropertyInfo> GetMacroProperties(Type type)
	    {
	        List<PropertyInfo> props = new List<PropertyInfo>();
	        var macroPropInterfaces = type.GetInterfaces().Where(t => typeof(IMacroProperties).IsAssignableFrom(t));
	        foreach (var interfaceType in macroPropInterfaces)
	        {
	            var typeProperties = interfaceType.GetProperties(
	                    BindingFlags.FlattenHierarchy
	                    | BindingFlags.Public
	                    | BindingFlags.Instance);
	            props.AddRange(typeProperties);
	        }	
	        return props;
	    }

        private string GetModuleParam(string parName)
        {
            Dictionary<string, string> localDictionary = SessionService.Get<Dictionary<string, string>>(controller.MacroId);
            return (localDictionary != null && localDictionary.ContainsKey(parName)) ? localDictionary[parName] : SessionService.Get<string>(parName);
        }
    }

    public class PopulateMacroPropertiesAttribute : ActionFilterAttribute
    {
        public ISessionService SessionService { get; set; }

        private const string MACRO_PARAMETERS = "macroParameters";
        private MacroController controller;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            controller = filterContext.Controller as MacroController;

            if (SessionService.Get<Dictionary<string, string>>(controller.MacroId) == null)
                SessionService.Set<Dictionary<string, string>>(controller.MacroId, new Dictionary<string, string>());

            if (filterContext.ActionParameters.Count > 0 && filterContext.ActionParameters.ContainsKey(MACRO_PARAMETERS))
            {
                var macroParameters = filterContext.ActionParameters[MACRO_PARAMETERS] as Dictionary<string, object>;
                SetMacroProperties(macroParameters);
            }
        }

        private void SetMacroProperties(Dictionary<string, object> macroParameters)
	    {
	        var props = GetMacroProperties(controller.GetType());
	
	        foreach (PropertyInfo prop in props)
	        {
	            if (macroParameters.ContainsKey(prop.Name))
	            {
	                prop.SetValue(controller, macroParameters[prop.Name] as string, null);
	                SetModuleParam(prop.Name, macroParameters[prop.Name] as string);
	            }
	        }
	    }

        private List<PropertyInfo> GetMacroProperties(Type type)
        {
            List<PropertyInfo> props = new List<PropertyInfo>();
            var macroPropInterfaces = type.GetInterfaces().Where(t => typeof(IMacroProperties).IsAssignableFrom(t));
            foreach (var interfaceType in macroPropInterfaces)
            {
                var typeProperties = interfaceType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);
                props.AddRange(typeProperties);
            }
            return props;
        }

        private void SetModuleParam(string parName, string parValue)
        {
            Dictionary<string, string> localDictionary = SessionService.Get<Dictionary<string, string>>(controller.MacroId);
            if (parValue != null && !string.IsNullOrEmpty(parValue))
            {
                if (localDictionary.ContainsKey(parName))
                    localDictionary[parName] = parValue;
                else
                    localDictionary.Add(parName, parValue);
            }
        }

    }
}