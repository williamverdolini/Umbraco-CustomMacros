using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomMacros.Areas.Sample.Models
{
    public class ToDoList
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<ToDoItem> Items { get; set; }
    }
}