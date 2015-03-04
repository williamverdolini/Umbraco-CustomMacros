using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CustomMacros.Areas.Framework.Commons;
using CustomMacros.Areas.Infrastructure.Commands;
using CustomMacros.Areas.Infrastructure.Controllers;

namespace CustomMacros.Areas.Infrastructure.Helpers
{
    public static class CommandHelpers
    {
        public static MvcHtmlString JsCommand(this HtmlHelper html, Command command)
        {
            string retStr = "";
            retStr += "new CustomMacros.commands.CommandMng.Command('" + command.Name + "', {";
            foreach (KeyValuePair<string, string> p in command.GetProperties())
            {
                if (p.Key != "Sender" && p.Key != "SenderId" && p.Key != "Context")
                    retStr += p.Key + " : " + p.Value + ",";
            }
            retStr += Constants.PARAM_COMMAND_TYPENAME + " : '" + command.Name + "'";
            retStr += "})";
            return new MvcHtmlString(retStr);
        }

        public static MvcHtmlString JsCommandsConfiguration(this HtmlHelper htmlHelper)
        {
            if (!(htmlHelper.ViewContext.Controller is MacroController))
                throw new Exception("Controller is NOT a MacroController. It's NOT Possibile to use HtmlHelper.JsCommandsConfiguration");

            var controller = htmlHelper.ViewContext.Controller as MacroController;

            //htmlHelper.Action("JsConfig", "LoginService", new { area = "Business", ModuleId = (ViewContext.Controller as eice.framework.Areas.Infrastructure.Controllers.WiNicBaseController).GetUniqueId() })
            string result = string.Empty;
            string jsConfiguration = controller.JsConfig();
            if (!string.IsNullOrEmpty(jsConfiguration))
                result = "<script type=\"text/javascript\">" + jsConfiguration + "</script>";

            return new MvcHtmlString(result);
        }

    }
}