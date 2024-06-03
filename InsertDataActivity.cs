using System;
using System.Data.Odbc;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public static class InsertDataActivity
{
    [FunctionName("InsertDataActivity")]
    public static async Task Run([ActivityTrigger] (string connectionString, int batchSize) input, ILogger log)
    {
        string connectionString = input.connectionString;
        int batchSize = input.batchSize;

        using (OdbcConnection connection = new OdbcConnection(connectionString))
        {
            try
            {
                connection.Open();
                StringBuilder insertQuery = new StringBuilder();
                insertQuery.Append("INSERT INTO maindev.default.OdbcWrittenCustomers (CustomerId, CustomerName, CustomerEmail, CustomerAddress) VALUES ");

                for (int i = 1; i <= 1000000; i++)
                {
                    insertQuery.Append($"({i}, 'Customer {i}', 'customer{i}@example.com', 'Address {i}'),");

                    if (i % batchSize == 0 || i == 1000000)
                    {
                        insertQuery.Length--; // Remove the trailing comma
                        insertQuery.Append(";");

                        using (OdbcCommand insertCmd = new OdbcCommand(insertQuery.ToString(), connection))
                        {
                            await insertCmd.ExecuteNonQueryAsync();
                        }

                        insertQuery.Clear();
                        insertQuery.Append("INSERT INTO maindev.default.OdbcWrittenCustomers (CustomerId, CustomerName, CustomerEmail, CustomerAddress) VALUES ");

                        if (i % batchSize == 0)
                        {
                            log.LogInformation($"{i} records inserted.");
                        }
                    }
                }

                log.LogInformation("All records inserted successfully.");
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred: {ex.Message}");
            }
        }
    }
}
