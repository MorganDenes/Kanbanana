﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbanana.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public string Text { get; set; }
    }
}
