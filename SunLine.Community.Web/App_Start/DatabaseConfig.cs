using Microsoft.WindowsAzure;
using SunLine.Community.Repositories;
using SunLine.Community.Services;

namespace SunLine.Community.Web
{
    public static class DatabaseConfig
    {
        public static void Register()
        {
            SetSqlAzureExecutionStrategy();
            DatabaseSetup.Init();
        }

        private static void SetSqlAzureExecutionStrategy()
        {
            string azureExecutionStrategy = CloudConfigurationManager.GetSetting("SqlAzureExecutionStrategyEnabled");
            ModelConfiguration.SqlAzureExecutionStrategyEnabled = azureExecutionStrategy == "true";
        }
    }
}