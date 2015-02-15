using System;
using System.Collections.Generic;
using System.Linq;
using CustomMacros.Areas.Infrastructure.Commons;
using CustomMacros.Areas.Sample.Models;
using CustomMacros.Areas.Sample.Services;

namespace CustomMacros.Areas.Sample.Worker
{
    public class ToDoListWorker : IToDoListWorker
    {
        private readonly IDataBase dataBase;

        public ToDoListWorker(IDataBase dataBase)
        {
            Contract.Requires<ArgumentNullException>(dataBase != null, "dataBase");
            this.dataBase = dataBase;
        }

        public IList<ToDoListViewModel> GetLists()
        {
            IList<ToDoListViewModel> listsViewModel = new List<ToDoListViewModel>();
            var lists = dataBase.GetLists();
            //very simple mapping
            lists.ToList().ForEach(list => {
                var listViewModel = MapToListViewModel(list);
                if (list.Items != null && list.Items.Count>0)
                    list.Items.ToList().ForEach(item => listViewModel.Items.Add(MapToItemViewModel(item)));                
                listsViewModel.Add(MapToListViewModel(list));
            });
            return listsViewModel;
        }

        public ToDoListViewModel GetList(int id)
        {
            ToDoList list = dataBase.GetList(id);
            //very simple mapping
            var listViewModel = MapToListViewModel(list);
            if (list.Items != null && list.Items.Count > 0)
                list.Items.ToList().ForEach(item => listViewModel.Items.Add(MapToItemViewModel(item)));
            return listViewModel;
        }

        public void ArchiveList(int id)
        {
            dataBase.ArchiveList(id);
        }

        private static ToDoListViewModel MapToListViewModel(ToDoList list)
        {
            return new ToDoListViewModel
            {
                id = list.id,
                Title = list.Title,
                Description = list.Description,
                Items = new List<ToDoItemViewModel>()
            };
        }

        private static ToDoItemViewModel MapToItemViewModel(ToDoItem item)
        {
            return new ToDoItemViewModel
            {
                Id = item.Id,
                Description = item.Description,
                CreationDate = item.CreationDate,
                Importance = item.Importance
            };
        }
    }
}