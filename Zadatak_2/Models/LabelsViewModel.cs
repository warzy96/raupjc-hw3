using System;
using System.Collections.Generic;
using Zadatak_1;

namespace Zadatak_2.Models
{
    public class LabelsViewModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public List<TodoItem> LabelTodoItems { get; set; }

        public LabelsViewModel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }
    }
}
