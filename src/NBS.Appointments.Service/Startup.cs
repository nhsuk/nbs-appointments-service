using Microsoft.ApplicationInsights.Extensibility.EventCounterCollector;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NBS.Appointments.Service.Core;
using NBS.Appointments.Service.Filters;
using Serilog;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace NBS.Appointments.Service
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
            var sessionManagerConfig = Configuration.GetSection("SessionManager").Get<SessionManagerOptions>();
            services
                .Configure<QflowOptions>(Configuration.GetSection("Qflow"))
                .Configure<DateTimeProviderOptions>(Configuration.GetSection("DateTimeProvider"))
                .Configure<ApiKeyAuthenticationOptions>(options => options.ApiKey = Configuration.GetValue<string>("NbsApiKey"));

            services
                .AddApplicationInsightsTelemetry()
                .AddHttpClient()
                .AddHttpContextAccessor()
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(CreateErrorInfo(context.ModelState));
                });

            services
                .AddQflowClient()
                .AddSessionManager(sessionManagerConfig)
                .AddDateTimeProvider()
                .RegisterValidators()
                .AddHelpers()
                .AddSwaggerGen(config =>
                {
                    config.OperationFilter<SwaggerHeaderFilter>();
                });

            services
                .AddAuthentication("ApiKey") 
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey",opts => { });
        }

        private IEnumerable<string> CreateErrorInfo(ModelStateDictionary modelState)
        {
            var errors = new List<string>();
            foreach (var key in modelState.Keys)
            { 
                foreach(var error in modelState[key].Errors)
                {
                    errors.Add(error.ErrorMessage);
                }                
            }
            return errors;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseExceptionHandler(handler => {
                handler.Run(async context => {
                    var showExceptions = Configuration.GetValue<bool>("ShowException");
                    if (showExceptions)
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = Text.Plain;

                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                        await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
                        await context.Response.WriteAsync(exceptionHandlerPathFeature.Error.StackTrace);
                    }
                });
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers()
                    .RequireAuthorization();
            });
        }
    }
}
