using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kanbanana.Models
{
    public class UserCompany
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CompanyName { get; set; }
    }
}
