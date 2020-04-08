using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanbanana;
using Kanbanana.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kanbanana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var services = host.Services.CreateScope())
            {
                var dbContext = services.ServiceProvider.GetRequiredService<KanbananaDbContext>();
                var userMgr = services.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleMgr = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var companyRole = new IdentityRole("Company");
                var employeeRole = new IdentityRole("Employee");

                if (!dbContext.Roles.Any())
                {
                    roleMgr.CreateAsync(companyRole).GetAwaiter().GetResult();
                    roleMgr.CreateAsync(employeeRole).GetAwaiter().GetResult();
                }

                //if (!dbContext.Users.Any())
                //{
                //    var companyUser = new IdentityUser
                //    {
                //        UserName = "test",
                //        Email = "admin@test.com"
                //    };
                //    var result = userMgr.CreateAsync(companyUser, "test").GetAwaiter().GetResult();
                //    userMgr.AddToRoleAsync(companyUser, companyRole.Name).GetAwaiter().GetResult();
                //}
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
