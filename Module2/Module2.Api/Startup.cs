using Bootloader.AspNet.Extensions;
using Bootloader.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Module1.Client;
using Module2.Api.HealthChecks;

namespace Module2.Api
{
    public class Startup: IModuleStartup
    {
        public string ModuleName { get; } = "Module2";
        public string ModuleVersion { get; } = "v1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddModule1Client(Configuration.ToModule1ClientConfiguration());
            services.AddControllers();
            services.AddBootloaderSwagger(this);

            var healthChecksBuilder = services.AddHealthChecks();
            if (!Configuration.IsBootloaderHosting())
            {
                //In bootloader mode we can skip some health checks
                healthChecksBuilder.AddCheck<DependencyHealthCheck>("module1");
            }
        }
        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBootloaderSwaggerWithUI(this, Configuration);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                if (!Configuration.IsBootloaderHosting())
                    endpoints.AddHealthCheckConfig();
                endpoints.MapControllers();
            });
        }
    }
}