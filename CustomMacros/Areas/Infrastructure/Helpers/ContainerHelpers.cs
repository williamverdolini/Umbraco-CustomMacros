using System;
using System.IO;
using System.Web.Mvc;
using CustomMacros.Areas.Framework.Services.Page;
using CustomMacros.Areas.Infrastructure.Commons;
using CustomMacros.Areas.Infrastructure.Controllers;

namespace CustomMacros.Areas.Infrastructure.Helpers
{
    internal class CustomMacrosContainer : IDisposable
    {
        readonly TextWriter Writer;
        readonly bool isPopup;
        bool disposed;

        public CustomMacrosContainer(TextWriter writer, bool isPopup)
        {
            Contract.Requires<ArgumentNullException>(writer != null, "writer");
            Writer = writer;
            this.isPopup = isPopup;
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                if (isPopup)
                {
                    Writer.Write("</div>");
                    Writer.Write("</div>");
                    Writer.Write("</div>");
                }
                Writer.Write("</div>");
            }
            GC.SuppressFinalize(this);
        }
    }

    public static class ContainerHelpers
    {
        public static IDisposable Container(this HtmlHelper htmlHelper, string containerCssClasses = "", string loaderCssClasses = "")
        {
            MacroController controller = htmlHelper.ViewContext.Controller as MacroController;

            string containerAttributes = string.Empty;
            bool isPopup = Utilities.CastBool(controller.PopupMode);
            if (isPopup && !controller.IsCustomPostBack)
            {
                containerAttributes = "style=\"display:none;\" ";
                containerCssClasses = "modal fade";
            }

            return Container(htmlHelper, controller, containerCssClasses, containerAttributes, loaderCssClasses);
        }

        private static IDisposable Container(this HtmlHelper htmlHelper, MacroController controller, string containerCssClasses = "", string containerAttributes = "", string loaderCssClasses = "")
        {
            var writer = htmlHelper.ViewContext.Writer;
            writer.WriteLine("<div class=\"" + controller.MacroId + "_main CustomMacrosmain " + containerCssClasses + "\"  " + containerAttributes + " >");
            writer.WriteLine("<div class=\"" + controller.MacroId + "_loading CustomMacrosloading " + loaderCssClasses + "\" ></div>");

            bool isPopup = Utilities.CastBool(controller.PopupMode);
            if (isPopup)
            {
                writer.WriteLine("<div class=\"modal-dialog\" >");
                writer.WriteLine("<div class=\"modal-content\" >");
                writer.WriteLine("<div class=\"modal-body\" >");
            }

            return new CustomMacrosContainer(writer, isPopup);
        }
    }
}