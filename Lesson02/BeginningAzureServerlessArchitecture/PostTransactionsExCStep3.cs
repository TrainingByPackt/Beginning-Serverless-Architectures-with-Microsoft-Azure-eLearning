using System.Net;
using System.Net.Http;
using BeginningAzureServerlessArchitecture.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using Microsoft.Extensions.Logging;

namespace BeginningAzureServerlessArchitecture
{
    public static class PostTransactions
    {
        private static string applicationInsightsKey = TelemetryConfiguration.Active.InstrumentationKey =
            Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);

        private static TelemetryClient telemetry = new TelemetryClient
        {
            InstrumentationKey = applicationInsightsKey
        };

        [FunctionName("PostTransactions")]
        public static HttpResponseMessage Run([HttpTrigger(
            AuthorizationLevel.Anonymous,
            "post",
            Route = "transactions")] HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var message = req.Content.ReadAsStringAsync().Result;
            try
            {
                var transaction = JsonConvert.DeserializeObject<Transaction>(message);
                if (transaction == null) throw new Exception("None of the properties on the request message are valid or match.");
            }
            catch(Exception e)
            {
                log.LogError($"Invalid request received, content of request was: {message}");
                telemetry.TrackEvent("Bad request received");
                telemetry.TrackException(e);
            }

            return req.CreateResponse(HttpStatusCode.OK, $"You made a transaction of ${transaction.Amount}");
        }
    }
}
