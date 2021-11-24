using Bootloader.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Module1.Client;

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(ModuleName + "-" + ModuleVersion,
                    new OpenApiInfo {Title = ModuleName, Version = ModuleVersion});
            });
        }
        public void Configure(IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetService<IWebHostEnvironment>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c=>
                {
                    c.RouteTemplate = ModuleName+"/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ModuleName}-{ModuleVersion}/swagger.json", $"{ModuleName} {ModuleVersion}");
                    c.RoutePrefix = ModuleName+"/swagger";
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}