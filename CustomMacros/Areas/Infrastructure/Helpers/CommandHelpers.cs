using System.Collections.Generic;
using System.Web.Mvc;
using CustomMacros.Areas.Framework.Commons;
using CustomMacros.Areas.Infrastructure.Commands;

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
    }
}