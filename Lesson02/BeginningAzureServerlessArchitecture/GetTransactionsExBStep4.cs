using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BeginningAzureServerlessArchitecture.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace BeginningAzureServerlessArchitecture
{
    public static class GetTransactions
    {
        [FunctionName("GetTransactions")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                "post",
                Route = null
            )] HttpRequestMessage req,
           [DocumentDB(
                ConnectionStringSetting = "CosmosDBConnectionString"
            )] DocumentClient client,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Using the Document DB SDK. Setting MaxItemCount - -1 means return all results of queries.
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
