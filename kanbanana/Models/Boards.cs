using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Kanbanana.Models
{
    public class Boards
    {
        public int Id { get; set; }
        public string Title { get; set; }
        //public ICollection<IdentityUser> Users { get; set; }
    }
}
