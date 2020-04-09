using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbanana.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int BoardId { get; set; }
        public int ColumnId { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class DisplayEditTask
    {
        public Task Task { get; set; }
        public virtual ICollection<Column> Columns { get; set; }
    }
}
