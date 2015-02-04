using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Runtime.Remoting.Messaging;

namespace SunLine.Community.Repositories
{
    public class ModelConfiguration : DbConfiguration
    {
        public ModelConfiguration()
        {
            if (SqlAzureExecutionStrategyEnabled)
            {
                SetExecutionStrategy("System.Data.SqlClient", 
                    () => SuspendExecutionStrategy ? (IDbExecutionStrategy)new DefaultExecutionStrategy() : new SqlAzureExecutionStrategy());
            }

            SetHistoryContext("System.Data.SqlClient", (connection, defaultSchema) => new ExHistoryContext(connection, defaultSchema));
        }

        public static bool SqlAzureExecutionStrategyEnabled{ get; set; }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }
            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }
    } 
}
