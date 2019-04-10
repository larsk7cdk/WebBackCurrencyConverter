using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Halcyon.Web.HAL.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using WebBackCurrencyConverter.API.Repositories;
using WebBackCurrencyConverter.API.Services;

namespace WebBackCurrencyConverter.API
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
            services.AddHttpClient<ICurrencyRatesRepository, CurrencyRatesRepository>();
            services.AddSingleton<IExchangeService, ExchangeService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Currency Converter API",
                    Description = "Et API til udstilling af valuta kurser og omregning mellem valuta",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Lars Larsen",
                        Email = "lars@k7c.dk",
                        Url = string.Empty
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc(options =>
            {
                options.OutputFormatters.RemoveType<JsonOutputFormatter>();
                options.OutputFormatters.Add(new JsonHalOutputFormatter(new string[]
                {
                    "application/hal+json",
                }));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            var logger = loggerFactory.CreateLogger("Logs");
            logger.LogInformation("WEB API Started");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency Converter V1");
                c.RoutePrefix = string.Empty;
            });


            app.UseMvc();
        }
    }
}