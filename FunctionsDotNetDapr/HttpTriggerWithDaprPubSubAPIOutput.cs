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
    public static class HttpTriggerWithDaprPubSubAPIOutput
    {

        private static HttpClient httpClient = new HttpClient();

        [FunctionName("HttpTriggerWithDaprPubSubAPIOutput")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "mypubsubpublisher")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("HttpTriggerWithDaprPubSubAPIIOutput processed a request.");

            string requestBody = String.Empty;
            string unescaped = "";
            using (var streamReader = new StreamReader(req.Body))
            {
                var body = await streamReader.ReadToEndAsync();
                string jsonString = System.Text.Json.JsonSerializer.Serialize(body);

                unescaped = System.Text.RegularExpressions.Regex.Unescape(jsonString);

                log.LogInformation("Message or request data: {0}", unescaped);

                string daprPublishUri = "http://localhost:3500/v1.0/publish/mypubsub1/mytopic1";

                var content = new StringContent($"{jsonString}", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(daprPublishUri, content);
                log.LogInformation("Dapr REST API publisher response: " + response.ReasonPhrase);
            }

            return new OkObjectResult(unescaped);
        }
    }
}
