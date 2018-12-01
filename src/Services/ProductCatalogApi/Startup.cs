using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductCatalogApi.Data;
using ShoesOnContainers.Services.ProductCatalogApi;

namespace ProductCatalogApi
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
            services.Configure<CatalogSettings>(Configuration);
            services.AddDbContext<CatalogContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MSSQLConnection")));
            services.AddSwaggerGen(options => {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info{
                    Title = "SalEasy / Containers / Services / Product-Catalog Http Api",
                    Version = "v1",
                    Description = "This app is making crud operations for product-catalog relationship",
                    TermsOfService = "Tersm Of Service"
                });
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddLogging();

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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSwagger()
            .UseSwaggerUI(sw => {
                sw.SwaggerEndpoint($"/swagger/v1/swagger.json", "ProductCatalogApi Microservice");
            });
            app.UseMvc();
        }
    }
}
