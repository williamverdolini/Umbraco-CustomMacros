using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CustomMacros.Areas.Framework.Services.Page;
using CustomMacros.Areas.Infrastructure.Commands;
using CustomMacros.Areas.Infrastructure.Commons;
using CustomMacros.Areas.Infrastructure.Controllers;
using CustomMacros.Areas.Sample.Commands;
using CustomMacros.Areas.Sample.Models;
using CustomMacros.Areas.Sample.Worker;
using Umbraco.Web.Mvc;

namespace CustomMacros.Areas.Sample.Controllers
{
    [PluginController("Sample")]
    public class ToDoListController : MacroController,
        // Command Provider
        ICommandProvider<SelectToDoListCommand>,
        ICommandProvider<ArchiveToDoListCommand>,
        ICommandProvider<OrderToDoListsCommand>,
        // Command Handler
        ICommandHandler<ArchiveToDoListCommand>,
        ICommandHandler<OrderToDoListsCommand>,
        // Macro Properties
        IToDoListMacroProperties
    {
        public string IsIdVisible { get; set; }

        #region Constructors
        private readonly IToDoListWorker worker;

        public ToDoListController(
            IToDoListWorker worker, 
            ICommandFactory commandFactory,
            IPageService pageService)
            : base(commandFactory, pageService)
        {
            Contract.Requires<ArgumentNullException>(worker != null, "worker");
            this.worker = worker;
        }
        #endregion

        #region Custom Actions
        public override ActionResult Handle(NoCommand command)
        {
            ViewBag.SortedFieldname = "id";
            ViewBag.IsAscending = false;
            ViewBag.IsIdVisible = Utilities.CastBool(IsIdVisible);
            return PartialView(worker.GetLists().OrderBy(list => list.id).ToList());
        }

        public ActionResult Handle(ArchiveToDoListCommand command)
        {
            worker.ArchiveList(Utilities.CastInt(command.ToDoListId));
            ViewBag.SortedFieldname = "id";
            ViewBag.IsAscending = true;
            return PartialView(worker.GetLists().OrderBy(list => list.id).ToList());
        }

        public ActionResult Handle(OrderToDoListsCommand command)
        {
            bool isAscending = Utilities.CastBool(command.IsAscending);
            ViewBag.SortedFieldname = command.FieldName;
            ViewBag.IsAscending = !isAscending;
            ViewBag.IsIdVisible = Utilities.CastBool(IsIdVisible);
            return PartialView(
                isAscending ?
                worker.GetLists().OrderBy(orderChoise[command.FieldName]).ToList() :
                worker.GetLists().OrderByDescending(orderChoise[command.FieldName]).ToList()
                );
        }
        #endregion

        #region Private logic
        private Dictionary<string, Func<ToDoListViewModel, object>> orderChoise = new Dictionary<string, Func<ToDoListViewModel, object>>
            {
                {"id", list => list.id },
                {"Title", list => list.Title },
                {"Description", List => List.Description}
            };
        #endregion
    }
}
