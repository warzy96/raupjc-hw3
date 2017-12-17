using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zadatak_1;

namespace Zadatak_2.Models
{
    public class LabelsViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Value { get; set; }
        public List<TodoItem> LabelTodoItems { get; set; }
        public string ErrorMessage { get; set; }

        public LabelsViewModel()
        {
            LabelTodoItems = new List<TodoItem>();
        }
        public LabelsViewModel(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
            LabelTodoItems = new List<TodoItem>();
        }
    }
}
