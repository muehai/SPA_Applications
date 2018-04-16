using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MeetingSchema.Data.Abstract;
using MeetingSchema.Data.Repositories;
using MeetingSchema.Data;
using System.Net;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using MeetingSchema.API.ViewModels.Mappings;

namespace MeetingSchema.API
{
    public class Startup
    {
        private static string Restful_ApplicationPath = string.Empty;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Restful_ApplicationPath = env.WebRootPath;

            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();


            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }
        }

        //This method are called by the CIL. This method use to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeetingSchemaContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //MeetingSchemas repositories
            services.AddScoped<IMeetingSchemasRepository, MeetingSchemasRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IParticipantsRepository, ParticipantsReopsitory>();

            //Automapper configuration for DI
            AutoMapperConfiguration.Configure();
           
            //Enable Cors CIL
            services.AddCors();
          
            //Add MVC services to the services container of Resolver.
            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    //Enable serialize to JSON using Camel
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }

        //This method gets called by the runtime CIL. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, MeetingSchemaContext contextDb)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<MeetingSchemaContext>().Database.Migrate();
                }
            }
            catch
            {
                //Catch error here if scope configuration get error
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            // Add MVC to the request pipeline.
            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseExceptionHandler(
              builder =>
              {
                  builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            //context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });
              });

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Uncomment the following line to add a route for porting Web API 2 controllers.
                //routes.MapWebApiRoute("DefaultApi", "api/{controller}/{id?}");
            });


            /*Initialize the MeetingSchema database using EF 
            * Comment out when you populate the database first time
            */
            //MeetingSchemaDbInitializer.Initialize(contextDb);
            
        }
    }
}
