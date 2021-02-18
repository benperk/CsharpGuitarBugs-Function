using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

using System.Collections.Generic;
using System.Threading;

namespace csharpguitarbugs_function
{
    public static class e
    {
        [FunctionName("e")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] 
            HttpRequest req, CancellationToken cancellationToken,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try 
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                List<string> random = new List<string>();
                Random r = new Random();
                var length = 700;
                var n = r.Next(1, 100);
                if (n % 5 == 0) length = 400;

                for (int i = 0; i < length; i++)
                {
                    random.Add(System.Guid.NewGuid().ToString());
                    await Task.Delay(i, cancellationToken);
                    if (i % 100 == 0)
                    {
                        log.LogInformation($"The code might be making an outbound HTTP connection, maybe...not sure");
                        log.LogInformation($"Also, to keep you up to speed on the processing, it's still working, we are on number {i}");
                        TimeSpan ts = stopWatch.Elapsed;
                        log.LogInformation($"The elapsed time is {ts.TotalSeconds} seconds so far...");
                        if (i > 300)
                        {
                            random.Sort();
                            random.Reverse();
                            var numberBreak = r.Next(1, 100);
                            if (numberBreak % 2 == 0) break;
                        }
                    }
                }
                random.Sort();
                random.Reverse();
                random.RemoveAt(100);
                random.Reverse();

                stopWatch.Stop();

                var number = r.Next(1, 100);
                if (number % 5 == 0)
                {
                    throw new System.Net.Sockets.SocketException();
                }
                return new OkObjectResult("That wasn't very fun, but it worked, but it worked badly, what's wrong?");
            }
            catch (System.Net.Sockets.SocketException seex)
            {
                return new BadRequestObjectResult($"The exception stack is {seex.StackTrace}, but there is no reason for this to happen I think.");
            }
            catch (System.Exception ex)
            {
                log.LogInformation(ex.Message);
                return new BadRequestObjectResult("This shouldn't ever happen, but if it does, please help me fix it, thanks.");
            }
        }
    }
}
