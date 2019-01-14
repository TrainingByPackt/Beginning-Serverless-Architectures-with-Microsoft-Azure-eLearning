using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BeginningAzureServerlessArchitecture.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;

namespace BeginningAzureServerlessArchitecture
{
    public static class PostTransactionsExCStep3
    {
        private static string key = TelemetryConfiguration.Active.InstrumentationKey =
            Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", EnvironmentVariableTarget.Process);

        private static TelemetryClient telemetry = new TelemetryClient
        {
            InstrumentationKey = key
        };

        [FunctionName("PostTransactions")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "transactions")]HttpRequestMessage req, 
            [DocumentDB("AzureServerlessArchitectureCourse","Transactions", ConnectionStringSetting = "CosmosDBConnectionString")] out Transaction transaction,
            ILogger log)
        {
            // ILogger allows you to do structured logs. An information log would simply inform of an event occurring, a warning log would flag potential issues and a critical log would record a serious problem
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            
            var message = req.Content.ReadAsStringAsync().Result;

            log.LogDebug($"Message received: {0}",message);

            try
            {
                transaction = JsonConvert.DeserializeObject<Transaction>(message);
                if (transaction == null) throw new Exception("None of the properties on the request message are valid or match.");
            }
            catch(Exception e)
            {
                log.LogError("No valid object received, Message was: {0}", message);
                telemetry.TrackEvent("Bad Request Received");
                telemetry.TrackException(e);

                transaction = null;
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "The request did not match the required schema");
            }

            log.LogInformation("Request successful");
            return req.CreateResponse(HttpStatusCode.OK, $"You made a transaction of £{transaction.Amount}!");
            
        }
        
    }
}
