using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarboraElevator.Model.Configuration;
using BarboraElevator.Services;
using BarboraElevator.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BarboraElevator
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
            services.AddControllers();

            services.AddTransient<IElevatorRouteService, ElevatorRouteService>();
            services.AddTransient<IElevatorEventLogService, ElevatorEventLogService>();
            services.AddTransient<IElevatorStatusService, ElevatorStatusService>();
            services.AddTransient<IElevatorControlService, ElevatorControlService>();
            services.AddTransient<IBuildingConfigurationService, BuildingConfigurationService>();
            services.AddTransient<IRouteValidationService, RouteValidationService>();

            services.AddSingleton<IElevatorPoolService, ElevatorPoolService>();
            services.AddSingleton(Configuration.GetSection("BuildingConfiguration").Get<BuildingConfiguration>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
