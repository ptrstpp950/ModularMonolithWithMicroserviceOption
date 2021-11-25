using System;
using Microsoft.Extensions.Configuration;

namespace Bootloader.AspNet.Extensions
{
    public static class EnvExtensions
    {
        public static bool IsBootloaderHosting(this IConfiguration config)
        {
            return "true".Equals(config["BootloaderHosting"], StringComparison.OrdinalIgnoreCase);
        }
    }
}