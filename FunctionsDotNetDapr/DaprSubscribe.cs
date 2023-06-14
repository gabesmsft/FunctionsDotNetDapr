using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FunctionsDotNetDapr
{
    public static class DaprSubscribe
    {
        [FunctionName("DaprSubscribe")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "dapr/subscribe")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Dapr Subscribe function processed a request.");

            string subscriptionInfo = $"[{{\"pubsubname\": \"mypubsub1\", \"topic\": \"mytopic1\", \"route\": \"/mypubsubconsumer\"}}]";

            return new OkObjectResult(subscriptionInfo);
        }
    }
}
