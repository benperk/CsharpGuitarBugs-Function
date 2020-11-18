using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace csharpguitarbugs_function
{
    public static class d
    {
        [FunctionName("d")]
        public static void Run([QueueTrigger("elx", Connection = "STORAGE_QUEUE_CONNECTION_STRING")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            try 
            {
                Random r = new Random();
                var n = r.Next(1, 20);
                string manufacturers = String.Empty;

                if (n % 2 == 0)
                {                    
                    for (int i = 0; i < 25; i++)
                    {
                        manufacturers += manufacturers + "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
                        //log.LogInformation($"Memory consumption = {System.GC.GetTotalMemory(false) / 1000000} MB.");
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                else
                {
                    var m = r.Next(1, 100);
                    int j = 10;
                    int k = 500;
                    if (m % 5 == 0)
                    {
                        j = 5;
                        k = 750;
                    }
                    for (int i = 0; i < j; i++)
                    {
                        manufacturers += manufacturers + "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890";
                        //log.LogInformation($"Memory consumption = {System.GC.GetTotalMemory(false) / 1000000} MB.");
                        System.Threading.Thread.Sleep(k);
                    }
                }
            }
            catch (System.Exception ex)
            {
                log.LogInformation($"A handled excpetion happened.");
                log.LogInformation($"Why did this happen and how can I resolve it or find out the issue?");
            }
        }
    }
}
