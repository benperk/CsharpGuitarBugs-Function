using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace csharpguitarbugs_function
{
    public static class f
    {
        [FunctionName("f")]
        public static void Run([CosmosDBTrigger(
            databaseName: "cshaprguitar",
            collectionName: "elx",
            ConnectionStringSetting = "COSMOS_DB_CONNECTION_STRING",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
        {
            List<string> random = new List<string>();

            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
                Random r = new Random();
                var n = r.Next(1, 100);
                if (n % 10 == 0) throw new Microsoft.Azure.WebJobs.Host.FunctionTimeoutException("Something unexpected happened...help me fix it.");

                for (int i = 0; i < 2000; i++)
                {
                    random.Add(System.Guid.NewGuid().ToString());
                    System.Threading.Thread.Sleep(i);
                    if (i % 100 == 0)
                    {
                        log.LogInformation($"Just keeping you up to speed on the processing, still working, we are on number {i}");
                    }
                }
            }
            random.Sort();
            random.Reverse();
            random.RemoveAt(500);
            random.Reverse();
        }
    }
}
