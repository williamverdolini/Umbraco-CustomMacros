using System.Collections.Generic;

namespace CustomMacros.Areas.Sample.Models
{
    public class ToDoListViewModel
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IList<ToDoItemViewModel> Items { get; set; }
    }
}