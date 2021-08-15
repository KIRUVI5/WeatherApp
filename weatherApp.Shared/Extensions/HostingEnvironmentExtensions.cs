using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace weatherApp.Shared.Extensions
{
    /// <summary>
    /// Extensions for the IHostingEnvironment largely to check if this is a live system
    /// </summary>
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// Any environments treated as live should be added here
        /// </summary>
        public static bool IsLive(this IHostingEnvironment env)
        {
            return env.IsProduction() || env.IsRegression();
        }

        /// <summary>
        /// Is this the regression environment
        /// </summary>
        public static bool IsRegression(this IHostingEnvironment env)
        {
            return env.IsEnvironment("Regression");
        }
    }
}
