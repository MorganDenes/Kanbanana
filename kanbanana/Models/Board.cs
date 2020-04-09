using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Kanbanana.Models
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Column> Columns { get; set; }
    }
}
