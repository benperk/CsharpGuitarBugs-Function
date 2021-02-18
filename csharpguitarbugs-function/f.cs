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
            databaseName: "csharpguitar",
            collectionName: "elx",
            ConnectionStringSetting = "COSMOS_DB_CONNECTION_STRING",
            LeaseCollectionName = "leases", 
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            List<string> random = new List<string>();

            if (input != null && input.Count > 0)
            {
                log.LogInformation($"Documents retrieved in this invocation is {input.Count}");
                
                Random r = new Random();
                var n = r.Next(1, 100);
                if (n % 10 == 0)
                {
                    try
                    {
                        throw new Microsoft.Azure.WebJobs.Host.FunctionTimeoutException("Something unexpected happened...help me fix it.");
                    }
                    catch (Exception ex)
                    {
                        log.LogInformation($"Exception {ex.HResult}, message: {ex.Message}");
                    }
                }

                string memoryTaker = "--WHAT-DOES-IMMUTABLE-MEAN";
                foreach (var document in input)
                {
                    n = r.Next(1, 100);
                    log.LogInformation($"Processing document with Id: {document.Id}");
                    log.LogInformation($"SelfLink: {document.SelfLink}");
                    log.LogInformation($"ResourceId: {document.ResourceId}");
                    log.LogInformation($"AltLink: {document.AltLink}");
                    log.LogInformation($"AttachmentsLink: {document.AttachmentsLink}");
                    log.LogInformation($"ETag: {document.ETag}");
                    log.LogInformation($"Timestamp: {document.Timestamp}");
                    log.LogInformation($"TimeToLive: {document.TimeToLive}");

                    if (n % 2 == 0)
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            memoryTaker += memoryTaker;
                            System.Threading.Thread.Sleep(10);
                            if (i % 50 == 0)
                            {
                                log.LogInformation($"We are on number: {i}, the length is: {memoryTaker.Length}");
                            }
                        }
                        if (n % 4 == 0)
                        {
                            try
                            {
                                throw new Microsoft.Azure.WebJobs.Host.RecoverableException("A transient issue happened, retrying...");
                            }
                            catch (Exception ex)
                            {
                                log.LogInformation($"A '{ex.HResult}' exception happened with a message: '{ex.Message}' so will retry...");
                                System.Threading.Thread.Sleep(1000);
                                log.LogInformation("Success... :-)    on to the next document...");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            random.Add(System.Guid.NewGuid().ToString());
                            var s = r.Next(10, 50);
                            System.Threading.Thread.Sleep(s);
                            if (i % 50 == 0)
                            {
                                log.LogInformation($"Just keeping you up to speed on the processing, still working, we are on number {i}");
                            }
                        }
                    }

                    if (n % 25 == 0) random.RemoveAt(1010);
                }                
            }
            random.Sort();
            random.Reverse();
            random.RemoveAt(50);
            random.Reverse();            
        }
    }
}
