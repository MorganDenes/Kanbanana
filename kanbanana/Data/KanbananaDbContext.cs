using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanbanana.Models;

namespace Kanbanana.Data
{
    // When adding own user type need to add <...> to IdentityDbContext
    public class KanbananaDbContext : IdentityDbContext
    {
        public KanbananaDbContext(DbContextOptions<KanbananaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Kanbanana.Models.Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserBoards> UserBoards { get; set; }
        public DbSet<UserCompany> UserCompanies { get; set; }
    }
}
