using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;

namespace Bootloader.AspNet.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void AddHealthCheckConfig(
            this IEndpointRouteBuilder endpoints)
        {
            // by default if Predicate is not set (is null) it will run all
            // checks. Therefore ready will execute all readiness checks.
            endpoints.MapHealthChecks("/healthz/ready", new HealthCheckOptions()
            {
                ResponseWriter = BootloaderHealthCheckWriters.WriteJson
            });

            // live will not execute any test and will just return 200 if its
            // up and running
            endpoints.MapHealthChecks("/healthz/live", new HealthCheckOptions()
            {
                Predicate = _ => false,
                ResponseWriter = BootloaderHealthCheckWriters.WriteJson
            });
        }
    }
}