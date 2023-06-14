using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDaprServiceInvocationAPIfrontend
    {

        private static HttpClient httpClient = new HttpClient();

        static string backendDaprAppId = System.Environment.GetEnvironmentVariable("backendDaprAppId") != null ? System.Environment.GetEnvironmentVariable("backendDaprAppId") : "functionapp1";

        [FunctionName("HttpTriggerWithDaprServiceInvocationAPIfrontend")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "daprserviceinvocationviaapi")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprServiceInvocationSDKfrontend Function processed a request.");

            string daprServiceInvocationBackendUri = @$"http://localhost:3500/v1.0/invoke/{backendDaprAppId}/method/daprserviceinvocationbackend";

            string responseBody;

            using (var response = httpClient.GetAsync(daprServiceInvocationBackendUri))
            {
                responseBody = response.Result.Content.ReadAsStringAsync().Result;
                log.LogInformation("Service invocation backend response is: " + responseBody);
            }

            return new OkObjectResult(responseBody);
        }
    }
}
