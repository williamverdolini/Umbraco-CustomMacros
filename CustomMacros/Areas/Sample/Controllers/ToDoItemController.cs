using System;
using System.Linq;
using System.Web.Mvc;
using CustomMacros.Areas.Framework.Services.Page;
using CustomMacros.Areas.Infrastructure.Commands;
using CustomMacros.Areas.Infrastructure.Commons;
using CustomMacros.Areas.Infrastructure.Controllers;
using CustomMacros.Areas.Sample.Commands;
using CustomMacros.Areas.Sample.Worker;
using Umbraco.Web.Mvc;

namespace CustomMacros.Areas.Sample.Controllers
{
    [PluginController("Sample")]
    public class ToDoItemController : MacroController,
        ICommandHandler<SelectToDoListCommand>
    {
        private readonly IToDoListWorker worker;

        public ToDoItemController(IToDoListWorker worker, ICommandFactory commandFactory, IPageService pageService)
            : base(commandFactory, pageService)
        {
            Contract.Requires<ArgumentNullException>(worker != null, "worker");
            this.worker = worker;
        }

        public ActionResult Handle(SelectToDoListCommand command)
        {
            int listId = Utilities.CastInt(command.ToDoListId);
            ViewBag.IsSelectedList = true;
            return PartialView(worker.GetList(listId));
        }

        public override ActionResult Handle(NoCommand command)
        {
            ViewBag.IsSelectedList = false;
            return PartialView();
        }
    }
}
