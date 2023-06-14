using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDapStateInputAPI
    {

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("HttpTriggerWithDapStateInputAPI")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "readfromstatestore")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDapStateInputAPI processed a request.");

            string stateStoreUri = "http://localhost:3500/v1.0/state/mystatestore/fakekey1";

            string responseBody;

            using (var response = await httpClient.GetAsync(stateStoreUri))
            {
                responseBody = await response.Content.ReadAsStringAsync();
                log.LogInformation("fakekey1 value: " + responseBody);
            }

            return new OkObjectResult("fakekey1 value: " + responseBody);
        }
    }
}
