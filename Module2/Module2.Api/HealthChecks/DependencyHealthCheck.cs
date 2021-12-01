using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Module1.Client;

namespace Module2.Api.HealthChecks
{
    public class DependencyHealthCheck: IHealthCheck
    {
        private readonly IModule1Client _module1Client;

        public DependencyHealthCheck(IModule1Client module1Client)
        {
            _module1Client = module1Client;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await _module1Client.CheckHealthCheck();
            return result ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy();
        }
    }
}