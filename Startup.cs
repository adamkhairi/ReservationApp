using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservationApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApp
{
     public class Startup
     {
          public Startup(IConfiguration configuration)
          {
               Configuration = configuration;
          }

          public IConfiguration Configuration { get; }


          public void ConfigureServices(IServiceCollection services)
          {
               services.AddHttpContextAccessor();
               services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseMySql(
                       Configuration.GetConnectionString("DefaultConnection")));
               services.AddDatabaseDeveloperPageExceptionFilter();

               services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                   .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>();
               services.AddControllersWithViews();
          }

          public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
          {
               if (env.IsDevelopment())
               {
                    app.UseDeveloperExceptionPage();
                    app.UseMigrationsEndPoint();
               }
               else
               {
                    app.UseExceptionHandler("/Home/Error");
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
                     name: "MyArea",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
               });
          }
     }
}
