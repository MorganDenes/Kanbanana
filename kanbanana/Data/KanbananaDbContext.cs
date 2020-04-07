using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbanana.Data
{
    // When adding own user type need to add <...> to IdentityDbContext
    public class KanbananaDbContext : IdentityDbContext
    {
        public KanbananaDbContext(DbContextOptions<KanbananaDbContext> options)
            : base(options)
        {
        }
    }
}
