using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace csharpguitarbugs_function
{
    public static class b
    {
        [FunctionName("b")]
        public static async Task Run([EventHubTrigger("elx", Connection = "EVENT_HUB_CONNECTION_STRING")] EventData[] events, ILogger log)
        {
            foreach (EventData eventData in events)
            {
                string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);
                log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");

                Random r = new Random();
                var zero = 0;
                var number = r.Next(1, 100);
                if (number % 2 == 0)
                {
                    var unCaughtZero = 1 / zero;
                }
                else
                {
                    log.LogInformation($"The Event Hub message: {messageBody} was processed successfully...this time.");
                }                
                
                await Task.Yield();                
            }
        }
    }
}
