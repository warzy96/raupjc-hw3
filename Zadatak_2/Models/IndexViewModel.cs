using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zadatak_1;

namespace Zadatak_2.Models
{
    public class IndexViewModel
    {
        public List<TodoViewModel> TodoList { get; set; }

        public IndexViewModel(List<TodoItem> list)
        {
            TodoList = new List<TodoViewModel>();
            foreach(var item in list)
            {
                var model = new TodoViewModel(item);
                TodoList.Add(model);
            }
        }
    }
}
