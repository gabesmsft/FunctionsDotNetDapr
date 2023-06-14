using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Dapr.Client;
using System.Net.Http;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDaprBindingSDKOutput
    {

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("HttpTriggerWithDaprBindingSDKOutput")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "bindingviasdk")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprBindingSDKOutput processed a request.");

            string requestBody = String.Empty;
            string unescaped = "";
            using (var streamReader = new StreamReader(req.Body))
            {
                var body = await streamReader.ReadToEndAsync();
                string jsonString = System.Text.Json.JsonSerializer.Serialize(body);

                unescaped = System.Text.RegularExpressions.Regex.Unescape(jsonString);

                log.LogInformation("Message or request data: {0}", unescaped);

                //The following lines of code use DaprClient SDK to make a request to the Dapr binding route for mybinding1, which will invoke the specified output binding.
                var daprClient = new DaprClientBuilder().Build();
                await daprClient.InvokeBindingAsync("mybinding1", "create", unescaped);

            }

            return new OkObjectResult(unescaped);
        }
    }
}
