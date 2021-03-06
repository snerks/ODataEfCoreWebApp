﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ODataEfCoreWebApp.Models;

using Microsoft.OData.Edm;
using Newtonsoft.Json;

namespace ODataEfCoreWebApp
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
            //Adding In Memory Database.
            services.AddDbContext<SampleODataDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDb");
            });

            ////Adding OData middleware.
            //services.AddOData();

            //services
            //    .AddMvc()
            //    //.SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            // https://www.youtube.com/watch?v=ZCDWUBOJ5FU
            // 0: csproj file
            // 1: Startup.cs : ConfigureServices
            // 2/3: Startup.cs : Configure
            // 4: PersonController.cs : [EnableQuery]
            // 4 Lines : 1
            services.AddOData();



            services.AddODataQueryFilter();

            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseMvc();

            // Adding Model class to OData
            var odataConventionModelBuilder = new ODataConventionModelBuilder();
            
            //odataConventionModelBuilder.EntitySet<Person>(nameof(Person));
            odataConventionModelBuilder
                .EntitySet<Person>(nameof(Person))
                .EntityType
                .Filter()
                .Count()
                .Expand()
                .OrderBy()
                .Page()
                .Select();

            var edmModel = odataConventionModelBuilder.GetEdmModel();

            // Enabling OData routing
            //app.UseMvc(
            //    routebuilder => 
            //    routebuilder.MapODataServiceRoute(
            //        "odata", 
            //        "odata",
            //        edmModel));

            // 4 Lines: 2/3
            app.UseMvc(
                routebuilder =>
                {
                    routebuilder.EnableDependencyInjection();
                    routebuilder.Expand().Select().Count().OrderBy().Filter();
                    routebuilder.MaxTop(100);
                });
        }
    }
}
