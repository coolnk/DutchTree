using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTree.Data;
using DutchTree.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DutchTree
{
    public class Startup
    {
	    private readonly IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _config = configuration;
            _env = env;
        }
		
		
		// This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
			//to add identity login, can also derive role classed form identity role
            services.AddIdentity<StoreUser, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
              //  cfg.Password.
            })
            .AddEntityFrameworkStores<DutchContext>(); //tilling where to get the store, some people like to have in a differnet context
      
            
            //requires to use dependency injection
	        services.AddDbContext<DutchContext>(cfg =>
	        {
		        cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
	        });

            //through dependency injection of automapper

            services.AddSingleton(
                new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<DutchMappingProfile>())));
            //Replaced the following caused mapper already initialized
            //services.AddAutoMapper();
			//scoped one within the scopde
			//singletime onece in the lifetime

			//Transient services are added as they are needed they are not held around, only need it once in a lifetime of an app 
			services.AddTransient<DutchSeeder>();
	        services.AddScoped<IDutchRepository, DutchRepository>();
            // Json options were added cuz of the order->orderitem-> order self referencing loop error, looking at the output window shows it all
            //The opt was added later to support ssl for using with authentication
			services.AddMvc(opt =>
			    {
			        if (_env.IsProduction())
			        {
			            opt.Filters.Add(new RequireHttpsAttribute());

			        }
			    }
                ).AddJsonOptions( opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //how doe sit know if a debug machine

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            


            ////whatever comes in just spit out this  
            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            //serve files --security protocol for websites
            //order matters here login, security asp.net pipeline under the covers 
            // add some middleware in the purpose
            // app.UseDefaultFiles(); // look for blandk directory url at the root url  look for those files by default
            app.UseStaticFiles();

            //since this is the pipeline authentication should come before the use MVC, the the wireup happening here
            app.UseAuthentication();

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", Action = "Index" });
            });

			//app.UseMvc(routes =>
			//{
			//    routes.MapRoute(
			//        name: "default",
			//        template: "{controller=App}/{action=Index}/{id?}");
			//});

	        if (env.IsDevelopment())
	        {
				//need to create a scope
		        using (var scope = app.ApplicationServices.CreateScope())
		        {
			        var seeder = scope.ServiceProvider.GetService<DutchSeeder>();
                    // this is async method call, we could convert the Configure to async 
                    // but the current version does not support well witl async so make it sync by adding .Wait() after seeder.Seed()
                    seeder.Seed();
		        }
	        }
        }
    }
}
