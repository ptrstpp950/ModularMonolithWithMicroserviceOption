using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bootloader.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bootloader
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private List<IModuleStartup> _moduleStartups;
        
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            _moduleStartups = ModuleLoader();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            foreach (var moduleStartup in _moduleStartups)
            {
                moduleStartup.ConfigureServices(services);
            }
        }

        private List<IModuleStartup> ModuleLoader()
        {
            var moduleRoot = System.AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(_configuration["moduleRoot"]))
                moduleRoot = _configuration["moduleRoot"];
            var assemblies = Directory.GetFiles(moduleRoot, "Module*.dll", SearchOption.AllDirectories)
                .Select(assemblyPath =>
                    System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath)).ToList();

            var typeList = new List<Type>();
            foreach (var assembly in assemblies)
            {
                try
                {
                    typeList.AddRange(
                        assembly.GetTypes().Where(type =>
                            typeof(IModuleStartup).IsAssignableFrom(type)
                            && !type.IsAbstract));
                }
                catch
                {
                    // ignored
                }
            }

            return typeList.Select(type =>
                    ActivatorUtilities.CreateInstance(new HostServiceProvider(_configuration, _env),
                        type))
                .ToList().Select(x => (IModuleStartup) x).ToList();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
            });
            
            foreach (var moduleStartup in _moduleStartups)
            {
                moduleStartup.Configure(app);
            }

        }
    }
    
    internal class HostServiceProvider : IServiceProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public HostServiceProvider(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IWebHostEnvironment) || serviceType == typeof(IHostEnvironment))
            {
                return _env;
            }

            if (serviceType == typeof(IConfiguration))
            {
                return _configuration;
            }

            return null;
        }
    }
}