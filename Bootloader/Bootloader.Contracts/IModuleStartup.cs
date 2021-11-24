using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Bootloader.Contracts
{
    public interface IModuleStartup
    {
        public string ModuleName { get; }
        public string ModuleVersion { get; }
        public void Configure(IApplicationBuilder app);
        public void ConfigureServices(IServiceCollection services);
    }
}