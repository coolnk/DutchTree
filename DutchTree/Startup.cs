using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTree.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace DutchTree
{
    public class Startup
    {
	    private readonly IConfiguration _config;

	    public Startup(IConfiguration configuration)
	    {
		    _config = configuration;
	    }
		
		
		// This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
			//requires to use dependency injection
	        services.AddDbContext<DutchContext>(cfg =>
	        {
		        cfg.UseSqlServer(_config.GetConnectionString("DutchConnectionString"));
	        });
			//scoped one within the scopde
			//singletime onece in the lifetime

			//Transient services are added as they are needed they are not held around, only need it once in a lifetime of an app 
			services.AddTransient<DutchSeeder>();
	        services.AddScoped<IDutchRepository, DutchRepository>();
            // Json options were added cuz of the order->orderitem-> order self referencing loop error, looking at the output window shows it all
			services.AddMvc().AddJsonOptions( opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
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
					seeder.Seed();
		        }
	        }
        }
    }
}
