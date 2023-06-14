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
    public static class HttpTriggerWithDaprPubSubAPITrigger
    {

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("HttpTriggerWithDaprPubSubAPITrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "mypubsubconsumer")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprPubSubAPITrigger processed a request.");

            string requestBody = String.Empty;
            string unescaped = "";
            using (var streamReader = new StreamReader(req.Body))
            {
                var body = await streamReader.ReadToEndAsync();
                string jsonString = System.Text.Json.JsonSerializer.Serialize(body);

                unescaped = System.Text.RegularExpressions.Regex.Unescape(jsonString);

                log.LogInformation("Message or request data: {0}", unescaped);
            }

            return new OkObjectResult(unescaped);
        }
    }
}
