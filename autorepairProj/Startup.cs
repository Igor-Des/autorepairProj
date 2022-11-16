using autorepairProj.Data;
using autorepairProj.Middleware;
using autorepairProj.Models;
using autorepairProj.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace autorepairProj
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
            services.AddMvc();
            services.AddControllersWithViews();

            string connectionDB = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AutorepairContext>(options => options.UseSqlServer(connectionDB));

            string connectionUserDB = Configuration.GetConnectionString("IdentityConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionDB));


            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();

            services.Configure<ApplicationDbContext>(o =>
            {
                o.Database.Migrate();
            });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddSession();
            services.AddScoped<ICached<Qualification>, CachedQualification>();
            services.AddScoped<ICached<Owner>, CachedOwners>();
            services.AddScoped<ICached<Mechanic>, CachedMechanics>();
            services.AddScoped<ICached<Car>, CachedCars>();
            services.AddScoped<ICached<Payment>, CachedPayments>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutorepairContext _context)
        {
            
            //serviceProvider.GetService<ApplicationDbContext>().Database.EnsureCreated();
            
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
            app.UseCookiePolicy();

            // ��������� ��������� ������
            app.UseSession();
            // ��������� ���������� miidleware �� ������������� ���� ������
            app.UseDbInitializer();

            app.UseRouting();


            // ������������� Identity
            app.UseAuthentication();
            app.UseAuthorization();



            _context.GetService<ICached<Qualification>>().AddList("Qualification");
            _context.GetService<ICached<Owner>>().AddList("Owner");
            _context.GetService<ICached<Mechanic>>().AddList("Mechanic");
            _context.GetService<ICached<Car>>().AddList("Car");
            _context.GetService<ICached<Payment>>().AddList("Payment");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
