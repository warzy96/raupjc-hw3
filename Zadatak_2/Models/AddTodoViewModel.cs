using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Zadatak_1;

namespace Zadatak_2.Models
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
        public DateTime? DateDue { get; set; }
        public List<LabelsViewModel> Labels { get; set; }
        public List<string> SelectedLabels { get; set; }
        public AddTodoViewModel()
        {
            Labels = new List<LabelsViewModel>();
        }
        public AddTodoViewModel(List<TodoItemLabel> labels)
        {
            Labels = new List<LabelsViewModel>();
            foreach (var label in labels)
            {
                Labels.Add(new LabelsViewModel(label.Value)
                {
                    Id = label.Id,
                    LabelTodoItems = label.LabelTodoItems
                });
            }
        }
    }
}
