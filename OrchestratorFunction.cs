using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public static class OrchestratorFunction
{
    [FunctionName("OrchestratorFunction")]
    public static async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var connectionString = "DSN=DatabricksWarehouse";
        var batchSize = 50000;
        var query = "SELECT c.customer_id, c.customer_name, c.customer_email, o.order_id, o.product, o.amount" +
                    " FROM Customers c" +
                    " JOIN Orders o ON c.customer_id = o.customer_id";

        // Call InsertDataActivity
        await context.CallActivityAsync("InsertDataActivity", (connectionString, batchSize));

        // Call GetDataActivity
        var result = await context.CallActivityAsync<List<string>>("GetDataActivity", (connectionString, query));

        return result;
    }
}
