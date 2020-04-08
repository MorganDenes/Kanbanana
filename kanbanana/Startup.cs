using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanbanana.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Kanbanana.Mail;
using System.Security.Claims;
using Kanbanana.AuthorizationRequirements;

namespace Kanbanana
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<KanbananaDbContext>(config => {
                config.UseInMemoryDatabase("Kanbanana");
            });

            // When adding own user type update IdentityUser to the correct class
            services.AddIdentity<IdentityUser, IdentityRole>(config => {
                config.Password.RequiredLength = 3; // !!! softening security for testing
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<KanbananaDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });

            //var mailJetOptions = Configuration.GetSection("MailJet)

            services.AddSingleton<MailJetInterface, MailJet>();
            services.Configure<MailJetOptions>(Configuration.GetSection("MailJet"));

            services.AddAuthorization(config =>
            {
                //var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuilder
                //    .RequireAuthenticatedUser()
                //    .Build();
                //config.DefaultPolicy = defaultAuthPolicy;
                config.AddPolicy("Company", policy => policy.RequireRole("Company"));

                config.AddPolicy("Employee", policy => policy.RequireAssertion(context =>
                                                        context.User.IsInRole("Company") ||
                                                        context.User.IsInRole("Employee")
                                                    ));

                //config.AddPolicy("Claim.Company", policyBuilder =>
                //{
                //    policyBuilder.RequireKanbananaClaim("Company").Build();
                //});
            });

            services.AddScoped<IAuthorizationHandler, KanbananaClaimHandler>();

            services.AddControllersWithViews();
            //services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
