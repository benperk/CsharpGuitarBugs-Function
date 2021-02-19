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

                int iteration = 1;
                int length = 1000;
                if (input.Count > 50 && input.Count <= 60) length = 450;
                if (input.Count > 60) length = 250;

                string memoryTaker = "--WHAT-DOES-IMMUTABLE-MEAN";
                foreach (var document in input)
                {                    
                    n = r.Next(1, 100);
                    log.LogInformation($"Processing document with Id: {document.Id} ({iteration} of {input.Count})");
                    log.LogInformation($"SelfLink: {document.SelfLink}");
                    log.LogInformation($"ResourceId: {document.ResourceId}");
                    log.LogInformation($"AltLink: {document.AltLink}");
                    log.LogInformation($"AttachmentsLink: {document.AttachmentsLink}");
                    log.LogInformation($"ETag: {document.ETag}");
                    log.LogInformation($"Timestamp: {document.Timestamp}");
                    log.LogInformation($"TimeToLive: {document.TimeToLive}");
                    iteration++;

                    if (n % 2 == 0)
                    {
                        for (int i = 0; i < length; i++)
                        {
                            memoryTaker = memoryTaker + "--WHAT-DOES-IMMUTABLE-MEAN--WHAT-DOES-IMMUTABLE-MEAN--WHAT-DOES-IMMUTABLE-MEAN";
                            System.Threading.Thread.Sleep(10);
                            if (i % 50 == 0)
                            {
                                log.LogInformation($"Processing, iteration: {i} consuming: {memoryTaker.Length}");
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
                                log.LogInformation($"Reprocessed document with Id: {document.Id}, success...");
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < length; i++)
                        {
                            System.Threading.Thread.Sleep(10);
                            if (i % 50 == 0)
                            {
                                log.LogInformation($"Processing, iteration: {i}");
                            }
                        }
                    }
                    if (n % 50 == 0) throw new AccessViolationException("Why did you do that?  You should know better");
                }                
            }           
        }
    }
}
