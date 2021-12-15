using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PartialViewSection.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartialViewSection
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
            services.AddControllersWithViews();
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromSeconds(20);
            });
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("VegeFoodCS")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            //services.ConfigureApplicationCookie(options=> {
            //    options.AccessDeniedPath = "/Admin/Account/Login";
            //});

            //services.AddAuthentication().AddCookie(options =>
            //{
            //    options.AccessDeniedPath = "/Admin/Account/Login";
            //});

            //services.Configure<IdentityOptions>(opt =>
            //{
            //    opt.Cookies.ApplicationCookie.AccessDeniedPath = new PathString("/Admin/Account/Login");
            //});

            services.Configure<CookieAuthenticationOptions>(options =>
            {
                options.AccessDeniedPath = new PathString("/Admin/Account/Login");
            });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseCookieAuthentication();

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //    {
            //        LoginPath = new PathString("admin/account/login/")
            //    });

            app.UseEndpoints(endpoints =>
            {

                //endpoints.MapControllerRoute(
                //       name: "areas",
                //       pattern: "admin/{controller=Home}/{action=Index}/{id?}"
                //   );

                //endpoints.MapControllerRoute(
                //    name: "MyArea",
                //    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "Areas",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Account}/{action=Login}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


            });
        }
    }
}
