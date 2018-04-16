using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using MeetingSchema.Data.Abstract;
using MeetingSchema.Data.Repositories;
using MeetingSchema.Data;
using System.Net;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using MeetingSchema.API.ViewModels.Mappings;

namespace AngularSPA
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            //if (env.IsDevelopment())
            //{
            //    // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
            //    builder.AddUserSecrets<Startup>();
            //}
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MeetingSchemaContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //MeetingSchema repositories
            services.AddScoped<IMeetingSchemasRepository, MeetingSchemasRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IParticipantsRepository, ParticipantsReopsitory>();

            //Automapper configuration for mapping data to viewmodels
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Added global configration to DbContext 
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
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/MS/Error");
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


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=MS}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "MS", action = "Index" });
            });
        }
    }
}
