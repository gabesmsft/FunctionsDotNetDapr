using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;

namespace FunctionsDotNetDapr
{
    public static class HttpTriggerWithDaprBindingAPITriggerAndOutput
    {

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("HttpTriggerWithDaprBindingAPITriggerAndOutput")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "mybinding1")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprBindingAPITriggerAndOutput processed a request.");

            string requestBody = String.Empty;
            string unescaped = "";
            using (var streamReader = new StreamReader(req.Body))
            {
                var body = await streamReader.ReadToEndAsync();
                string jsonString = System.Text.Json.JsonSerializer.Serialize(body);

                unescaped = System.Text.RegularExpressions.Regex.Unescape(jsonString);

                log.LogInformation("Message or request data: {0}", unescaped);

                string daprOutputBindingUri = "http://localhost:3500/v1.0/bindings/mybinding2";

                var content = new StringContent($"{{\"data\":{jsonString},\"operation\":\"create\"}}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(daprOutputBindingUri, content);
                log.LogInformation("Dapr REST API output binding response: " + response.ReasonPhrase);
            }

            return new OkObjectResult(unescaped);
        }
    }
}
