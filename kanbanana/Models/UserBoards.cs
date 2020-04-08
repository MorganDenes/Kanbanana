using Kanbanana.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbanana.Models
{
    public class UserBoards
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BoardId { get; set; }
    }
}
