﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zadatak_2.Models
{
    public class AddTodoViewModel
    {
        [Required]
        public string Text { get; set; }
        public DateTime? DateDue { get; set; }
        public string Labels { get; set; }
    }
}
