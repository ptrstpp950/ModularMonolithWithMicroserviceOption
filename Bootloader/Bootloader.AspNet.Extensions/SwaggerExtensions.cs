using System;
using Bootloader.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Bootloader.AspNet.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddBootloaderSwagger(
            this IServiceCollection services,
            IModuleStartup module)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(module.ModuleName + "-" + module.ModuleVersion,
                    new OpenApiInfo {Title = module.ModuleName, Version = module.ModuleVersion});
            });
            return services;
        }

        public static IApplicationBuilder UseBootloaderSwaggerWithUI(
            this IApplicationBuilder app,
            IModuleStartup module, IConfiguration config)
        {
            var moduleName = module.ModuleName;
            var moduleVersion = module.ModuleVersion;
            
            if (config.IsBootloaderHosting())
                return app;
            
            app.UseSwagger(c => { c.RouteTemplate = moduleName + "/swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{moduleName}-{moduleVersion}/swagger.json", $"{moduleName} {moduleVersion}");
                c.RoutePrefix = moduleName + "/swagger";
            });

            return app;
        }
        
    }
}