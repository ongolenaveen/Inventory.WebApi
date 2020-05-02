using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Inventory.Api;
using System;

namespace Inventory.WebApp
{
    /// <summary>
    /// Start Up Class
    /// </summary>
    public class Startup
    {
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configure Services.This method gets called by the runtime. 
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="services">Services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddApiVersioning((options) =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddSwaggerGen((options) => {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Inventory API",
                    Version = "v1",
                    Description = "Inventory API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Naveen Papisetty",
                        Email = string.Empty
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT"),
                    }
                });
            });
            var assembly = typeof(GroceriesController).Assembly;
            services.AddMvc((options) => {
                options.EnableEndpointRouting = false;
            })
                .AddApplicationPart(assembly);

            services.AddBindings(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">App</param>
        /// <param name="env">Environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseApiVersioning();

            app.UseCors((config) =>
            {
                config.AllowAnyOrigin();
                config.AllowAnyHeader();
                config.AllowAnyMethod();
            });

            app.UseSwagger();
            app.UseSwaggerUI((options) => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Inventory API V1");
            });
            app.UseMvc();
        }
    }
}
