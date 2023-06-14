using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDaprServiceInvocationbackend
    {

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("HttpTriggerWithDaprServiceInvocationbackend")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "daprserviceinvocationbackend")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprServiceInvocationbackendend Function processed a request.");

            return new OkObjectResult($"{{\"backend response\":\"success\"}}");
        }
    }
}
