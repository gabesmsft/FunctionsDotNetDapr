using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Text;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDapStateOutputAPI
    {

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("HttpTriggerWithDapStateOutputAPI")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "writetostatestore")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDapStateOutputAPI processed a request.");

            string value = "fakevalue " + DateTime.UtcNow;

            string stateStoreUri = "http://localhost:3500/v1.0/state/mystatestore/";

            var stateContent = $"[{{\"key\": \"fakekey1\", \"value\": \"{value}\"}}]";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, stateStoreUri);

            request.Content = new StringContent(stateContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.SendAsync(request);
            log.LogInformation($"Writing {value} to state store");

            return new OkObjectResult($"Writing {value} to state store");
        }
    }
}
