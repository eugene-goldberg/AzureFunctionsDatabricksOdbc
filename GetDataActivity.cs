using System;
using System.Collections.Generic;
using System.Data.Odbc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public static class GetDataActivity
{
    [FunctionName("GetDataActivity")]
    public static List<string> Run([ActivityTrigger] (string connectionString, string query) input, ILogger log)
    {
        string connectionString = input.connectionString;
        string query = input.query;
        List<string> records = new List<string>();

        using (OdbcConnection connection = new OdbcConnection(connectionString))
        {
            try
            {
                connection.Open();

                using (OdbcCommand command = new OdbcCommand(query, connection))
                {
                    using (OdbcDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string record = reader.GetString(0) + " " + reader.GetString(1) +
                                            " " + reader.GetString(2) + " " + reader.GetString(3) +
                                            " " + reader.GetString(4) + " " + reader.GetString(5);
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogError($"An error occurred: {ex.Message}");
            }
        }

        return records;
    }
}
