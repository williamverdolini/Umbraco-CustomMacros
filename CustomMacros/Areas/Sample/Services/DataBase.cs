using System;
using System.Collections.Generic;
using System.Linq;
using CustomMacros.Areas.Sample.Models;

namespace CustomMacros.Areas.Sample.Services
{
    public class DataBase : IDataBase
    {
        private IList<ToDoList> lists;
        private IList<ToDoList> archivedLists;

        public DataBase()
        {
            lists = new List<ToDoList>{
                new ToDoList{id=1, Title="Training List", Description= "List of my Training priorities", 
                    Items = new List<ToDoItem>{
                        new ToDoItem{Id=1, CreationDate=DateTime.Now,Description="SignalR+Angular.js",Importance=100},
                        new ToDoItem{Id=2, CreationDate=DateTime.Now,Description="TeamCity",Importance=600},
                        new ToDoItem{Id=3, CreationDate=DateTime.Now,Description="Advanced .NET Caching",Importance=90}
                    } 
                },
                new ToDoList{id=2, Title="Christmas Gifts", Description= "List of gifts to buy for next Xmas", Items = new List<ToDoItem>{} },
                new ToDoList{id=3, Title="Cities to visit", Description= "for vacation, within next 2 years", Items = new List<ToDoItem>{} }
            };
            archivedLists = new List<ToDoList>();
        }

        public IList<ToDoList> GetLists()
        {
            return lists;
        }

        public ToDoList GetList(int id)
        {
            return lists.First(list => list.id.Equals(id));
        }

        public void ArchiveList(int id)
        {
            var listToRemove = lists.First(l => l.id.Equals(id));
            if (listToRemove != null)
            {
                archivedLists.Add(listToRemove);
                lists.Remove(listToRemove);
            }

        }

        public IList<ToDoItem> GetItems(int listId)
        {
            var list = lists.First(l => l.id.Equals(listId));
            return (list!= null) ? list.Items : new List<ToDoItem>();
        }

    }
}