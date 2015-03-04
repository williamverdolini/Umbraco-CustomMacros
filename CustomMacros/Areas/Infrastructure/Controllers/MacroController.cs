using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CustomMacros.Areas.Framework.Services.Page;
using CustomMacros.Areas.Infrastructure.Commands;
using CustomMacros.Areas.Infrastructure.Commons;
using CustomMacros.Areas.Infrastructure.Filters;
using Umbraco.Web.Mvc;

namespace CustomMacros.Areas.Infrastructure.Controllers
{
    [SessionExpiredException(Order = 20)]
    [LogException(RedirectTo = "/Errore", Order = 10)]
    [RetrieveMacroProperties(Order = 20)]
    [SubscribeCommandsToProvide(Order = 40)]
    [SubscribeCommandsToHandle(Order = 50)]
    public abstract class MacroController : SurfaceController, IMacroProperties
    {
        #region local Constans
        private const string COMMAND_ACTION = "Execute";
        private const string RESULT_DIV = "divResult";
        private const string LOADING_DIV = "divLoading";
        #endregion

        #region Constructors
        private readonly ICommandFactory commandFactory;
        private readonly IPageService pageService;

        public MacroController(ICommandFactory commandFactory, IPageService pageService)
        {
            Contract.Requires<ArgumentNullException>(commandFactory != null, "commandFactory");
            Contract.Requires<ArgumentNullException>(pageService != null, "pageService");
            this.commandFactory = commandFactory;
            this.commandFactory.MacroName = this.GetType().Name;
            this.pageService = pageService;
            CommandsConsumer = new List<Command>();
            MacroId = this.GetType().Name;
        }
        #endregion

        #region Public Properties and Methods
        public string MacroId { get; set; }
        public ICommandFactory CommandsProvider { get { return commandFactory; } }
        public IList<Command> CommandsConsumer { get; set; }
        public string PopupMode { get; set; }
        public string PageAbsolutePath { get; set; }
        public string GetLanguage()
        {
            return pageService.GetCultureInfo(PageAbsolutePath).TwoLetterISOLanguageName;
        }
        public bool IsCustomPostBack { get; set; }
        public string JsConfig()
        {
            return SetCommandListeners();
        }

        #endregion

        #region Global Filters
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            IsCustomPostBack = GetIsCustomPostBack(filterContext.ActionDescriptor.ActionName);
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region Actions
        [MapToView]
        [PopulateMacroProperties(Order = 10)]
        public ActionResult Init(IDictionary<string, object> MacroParameters)
        {
            return HandleCommand(GetBusinessCommand());
        }

        [HttpPost]
        [MapToView(ViewName = "Handle")]
        public ActionResult Execute()
        {
            return HandleCommand(GetBusinessCommand());
        }

        public abstract ActionResult Handle(NoCommand command);
        #endregion

        #region Private methods
        private Command GetBusinessCommand()
        {
            Command command = commandFactory.CreateFromRequest();
            command = (command != null && CommandsConsumer.Any(e => e.GetType().Equals(command.GetType()))) ? command : new NoCommand();
            return command;
        }

        private ActionResult HandleCommand<T>(T command)
        {
            dynamic handler = this;
            return (ActionResult)handler.Handle((dynamic)command);
        }

        private string GetExecuteActionUrl()
        {
            var controllerName = ControllerContext.RouteData.Values["Controller"];
            var area = ControllerContext.RouteData.DataTokens["area"];
            string actionUrl = area != null ?
                Url.Action(COMMAND_ACTION, controllerName.ToString(), new { area = area.ToString() }) :
                Url.Action(COMMAND_ACTION, controllerName.ToString());
            return actionUrl;
        }

        private string SetCommandListeners()
        {
            string jqGetCommand = "";
            string macroName = MacroId;
            string divResult = macroName + "_main";
            string divLoading = macroName + "_loading";

            string _actionUrl = GetExecuteActionUrl();

            jqGetCommand += "\n";
            jqGetCommand += Utilities.GetJsCommandsListener(CommandsConsumer, macroName);
            jqGetCommand += "\n" +
                            "window['" + macroName + "'] = {};\n" +
                            "window['" + macroName + "'].fireAjax = function (moduleName, commandSet) {\n" +
                            "  if($('." + divResult + ":first').length>0) {\n" +
                            "    var data = { \n" +
                            "          " + LOADING_DIV + ":'" + divLoading + "', " + "\n" +
                            "          " + RESULT_DIV + ":'" + divResult + "'" + "\n" +
                            "    } \n" +
                            "    $.extend(data, commandSet);\n";

            if (Utilities.CastBool(PopupMode))
                jqGetCommand += "    CustomMacros.ajax.executeInPopup(data, '" + _actionUrl + "', window['" + macroName + "_dialog']); " + "\n";
            else
                jqGetCommand += "    CustomMacros.ajax.execute(data, '" + _actionUrl + "'); " + "\n";
            jqGetCommand += "  }\n" +
                            "}\n";

            return (CommandsConsumer.Count > 0) ? jqGetCommand : string.Empty;
        }

        private bool GetIsCustomPostBack(string ActionName)
        {
            return !string.IsNullOrEmpty(ActionName) && ActionName.Equals(COMMAND_ACTION);
        }
        #endregion
    }
}