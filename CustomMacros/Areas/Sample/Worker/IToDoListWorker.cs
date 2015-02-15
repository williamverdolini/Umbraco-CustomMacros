using System.Collections.Generic;
using CustomMacros.Areas.Infrastructure.Controllers;
using CustomMacros.Areas.Sample.Models;

namespace CustomMacros.Areas.Sample.Worker
{
    public interface IToDoListWorker : IWorker
    {
        IList<ToDoListViewModel> GetLists();
        ToDoListViewModel GetList(int id);
        void ArchiveList(int id);
    }
}
