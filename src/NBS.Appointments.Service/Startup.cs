using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NBS.Appointments.Service.Core;
using System.Collections.Generic;

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
            services.Configure<QflowOptions>(Configuration.GetSection("Qflow"));
            var dateTimeProvider = Configuration.GetValue<string>("DateTimeProvider", "system");

            services.AddHttpClient();
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(CreateErrorInfo(context.ModelState));
            });

            services
                .AddQflowClient()
                .AddInMemoryStoreMutex()
                .AddDateTimeProvider(dateTimeProvider)
                .RegisterValidators()
                .AddSwaggerGen();
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
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
