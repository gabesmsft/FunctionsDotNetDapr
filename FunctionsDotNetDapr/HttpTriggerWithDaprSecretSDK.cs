using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Dapr.Client;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDaprSecretSDK
    {
        [Disable("DisableDaprSecretComponent")]
        [FunctionName("HttpTriggerWithDaprSecretSDK")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "mysecretviadaprsdk")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprSecretSDK processed a request.");

            //The following lines of code use DaprClient SDK to make a REST request to the Dapr secret store route http://localhost:3500/v1.0/secrets/mysecretstore/mykvsecret2.
            var daprClient = new DaprClientBuilder().Build();
            var secret = await daprClient.GetSecretAsync("mysecretstore", "mysecret2");

            log.LogInformation("My secret is: " + secret["mysecret2"]);

            return new OkObjectResult(secret["mysecret2"]);
        }
    }
}
