using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zadatak_1;

namespace Zadatak_2.Models
{
    public class CompletedViewModel
    {
        public List<TodoViewModel> TodoCompletedList { get; set; }

        public CompletedViewModel(List<TodoItem> list)
        {
            TodoCompletedList = new List<TodoViewModel>();
            foreach (var item in list)
            {
                var model = new TodoViewModel(item);
                TodoCompletedList.Add(model);
            }
        }
    }
}
