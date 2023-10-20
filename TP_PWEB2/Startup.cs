using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TP_PWEB2.Data;

namespace TP_PWEB2
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
            var dbpath = Path.Combine(Directory.GetCurrentDirectory(), "DB");

            var dbContext = services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection").Replace("[DataDirectory]", dbpath)));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            //Seed_Users.SeedAssinc(dbContext,  )
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateRolesAndUsers(serviceProvider).Wait();

        }



        private async Task CreateRolesAndUsers(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            string[] rolesNames = { "Admin", "Gestor", "Funcionario", "Cliente"};
            IdentityResult result;
            foreach (var namesRole in rolesNames){
                var roleExist = await roleManager.RoleExistsAsync(namesRole);
                if (!roleExist){
                    result = await roleManager.CreateAsync(new IdentityRole(namesRole));
                }
            }
            var userAdmin = new AppUser{
                UserName = "admin@pweb.pt",
                Email = "admin@pweb.pt",
                pNome = "Admin",
                uNome = "Da Plataforma",
                EmailConfirmed = true
            };

            if (userManager.Users.Where(u => u.UserName == userAdmin.UserName).Count() == 0)
            {
                result = await userManager.CreateAsync(userAdmin, "1qazZAQ!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userAdmin, "Admin");
                }
            }
        }

    }
}
