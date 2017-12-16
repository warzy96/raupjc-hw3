using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Threading.Tasks;
using Zadatak_1;

namespace Zadatak_2.Models
{
    public class TodoViewModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime? DateDue { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }

        public TodoViewModel()
        {
            //for entity framework
        }
        public TodoViewModel(TodoItem item)
        {
            Id = item.Id;
            Text = item.Text;
            DateDue = item.DateDue;
            DateCompleted = item.DateCompleted;
            DateCreated = item.DateCreated;
        }

        public string DaysUntilDeadline()
        {
            if (DateDue != null)
            {
                if (DateDue < DateTime.UtcNow) return "";
                return "(za " + (DateDue - DateTime.UtcNow).Value.Days + " dana!)";
            }
            return "";
        }
    }
}
