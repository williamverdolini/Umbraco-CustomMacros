using System.Collections.Generic;
using CustomMacros.Areas.Sample.Models;

namespace CustomMacros.Areas.Sample.Services
{
    public interface IDataBase
    {
        IList<ToDoList> GetLists();
        ToDoList GetList(int id);
        void ArchiveList(int id);
        //IList<ToDoItem> GetItems(int listId);
    }
}
