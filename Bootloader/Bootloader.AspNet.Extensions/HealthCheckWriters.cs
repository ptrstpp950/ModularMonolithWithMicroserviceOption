using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Bootloader.AspNet.Extensions
{
    public class BootloaderHealthCheckWriters
    {
        public static async Task WriteJson(HttpContext context, HealthReport report)
        {
            context.Response.ContentType = "application/json";
            var response = new HealthCheckReponse
            {
                Status = report.Status.ToString(),
                HealthChecks = report.Entries.Select(x => new IndividualHealthCheckResponse
                {
                    Component = x.Key,
                    Status = x.Value.Status.ToString(),
                    Description = x.Value.Description
                }),
                HealthCheckDuration = report.TotalDuration
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, Formatting.Indented));
        }
        public class IndividualHealthCheckResponse
        {
            public string Status { get; set; }
            public string Component { get; set; }
            public string Description { get; set; }
        }
        public class HealthCheckReponse
        {
            public string Status { get; set; }
            public IEnumerable<IndividualHealthCheckResponse> HealthChecks { get; set; }
            public TimeSpan HealthCheckDuration { get; set; }
        }
    }
}