using System.Configuration;
using System.Diagnostics;
using SunLine.Community.Repositories;
using SunLine.Community.Services;

namespace SunLine.Community.Web
{
    public static class DatabaseConfig
    {
        public static void Register()
        {
            Trace.TraceInformation("DatabaseConfig registering");

            SetSqlAzureExecutionStrategy();
            DatabaseSetup.Init();
        }

        private static void SetSqlAzureExecutionStrategy()
        {
            string azureExecutionStrategy = ConfigurationManager.AppSettings["SqlAzureExecutionStrategyEnabled"];
            ModelConfiguration.SqlAzureExecutionStrategyEnabled = azureExecutionStrategy == "true";
        }
    }
}