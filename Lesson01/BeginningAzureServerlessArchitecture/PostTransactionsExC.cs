using System.Net;
using System.Net.Http;
using BeginningAzureServerlessArchitecture.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace BeginningAzureServerlessArchitecture
{
    public static class PostTransactions
    {
        [FunctionName("PostTransactions")]
        public static HttpResponseMessage Run([HttpTrigger(
            AuthorizationLevel.Anonymous, 
            "post", 
            Route = "transactions")] HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var message = req.Content.ReadAsStringAsync().Result;
            var transaction = JsonConvert.DeserializeObject<Transaction>(message);

            return req.CreateResponse(HttpStatusCode.OK, $"You made a transaction of £{transaction.Amount}");
        }
    }
}
