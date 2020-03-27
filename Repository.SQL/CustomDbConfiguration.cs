using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SQL
{
    /*
     * The constructor of SqlAzureExecutionStrategy can accept two parameters, MaxRetryCount and MaxDelay. 
     * MaxRetry count is the maximum number of times that the strategy will retry. The MaxDelay is a TimeSpan 
     * representing the maximum delay between retries that the execution strategy will use. 
     * 
     * The SqlAzureExecutionStrategy will retry instantly the first time a transient failure occurs, but will 
     * delay longer between each retry until either the max retry limit is exceeded or the total time hits the max delay.
     */
    public class CustomDbConfiguration : DbConfiguration
    {
        public CustomDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy(3, TimeSpan.FromSeconds(5)));
        }
    }
}
