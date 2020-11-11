using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace csharpguitarbugs_function
{
    public static class c
    {
        [FunctionName("c")]
        public static void Run([ServiceBusTrigger("elx", Connection = "SERVICE_BUS_CONNECTION_STRING")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            List<string> manufacturers = new List<string>();

            Random r = new Random();
            //var count = 1;
            var n = r.Next(1, 100);
            ///*if (n % 5 == 0)*/ count = 3;

            if (n % 5 == 0)
            {
                manufacturers.Add(GetManufacturer("\"Gibson Fender Charvel Taylor Jackson MartinandCompany Dean Epiphone Takamine"));
            }                
            //for (int i = 0; i < count; i++)
            //{
            //    manufacturers.Add(GetManufacturer("\"Gibson Fender Charvel Taylor Jackson MartinandCompany Dean Epiphone Takamine"));
            //}
        }
        public static string GetManufacturer(string manufacturer)
        {
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(manufacturer, "\"(([^\\\\\"]*)(\\\\.)?)*\"");
            return m.ToString();
        }
    }
}
