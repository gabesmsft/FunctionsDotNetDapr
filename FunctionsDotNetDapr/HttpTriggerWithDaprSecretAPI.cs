using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDaprSecretAPI
    {

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("HttpTriggerWithDaprSecretAPI")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "mysecretviadaprapi")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprSecretAPI processed a request.");

            string daprSecretStoreUri = "http://localhost:3500/v1.0/secrets/mysecretstore/mysecret1";

            string responseBody;

            using (var response = await httpClient.GetAsync(daprSecretStoreUri))
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
                log.LogInformation("My secret is: " + responseBody);
            }

            return new OkObjectResult(responseBody);
        }
    }
}
