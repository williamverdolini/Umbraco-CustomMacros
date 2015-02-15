﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomMacros.Areas.Sample.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public int ToDoListId { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int Importance { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int UserId { get; set; }
    }
}