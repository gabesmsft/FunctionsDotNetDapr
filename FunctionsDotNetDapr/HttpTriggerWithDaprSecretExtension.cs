using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Dapr;
using System.Collections.Generic;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDaprSecretExtension
    {

        [FunctionName("HttpTriggerWithDaprSecretExtension")]
        public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "daprsecretextension")] HttpRequest req,
        [DaprSecret("mysecretstore", "mysecret3")] IDictionary<string, string> secret,
        ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprSecretExtension processed a request.");

            log.LogInformation("My secret is: " + secret["mysecret3"]);

            return new OkObjectResult(secret["mysecret3"]);
        }
    }
}
