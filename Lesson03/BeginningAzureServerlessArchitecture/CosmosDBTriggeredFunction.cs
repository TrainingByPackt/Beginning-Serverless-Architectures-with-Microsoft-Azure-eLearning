using System.Collections.Generic;
using System.Net.Http;
using BeginningAzureServerlessArchitecture.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace BeginningAzureServerlessArchitecture
{
    public static class CosmosDBTriggeredFunction
    {
        private static readonly string webAddress = "";
        [FunctionName("CosmosDBTriggeredFunction")]
        public static void Run([CosmosDBTrigger(
            databaseName: "AzureServerlessArchitectureCourse",
            collectionName: "transactions",
            ConnectionStringSetting = "CosmosDBConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, TraceWriter log)
        {
            if (input != null && input.Count > 0)
            {
                var httpClient = new HttpClient();
                foreach(var document in input)
                {
                    httpClient.PostAsync(webAddress, new StringContent(JsonConvert.SerializeObject((Transaction)(dynamic)document)));
                }
            }
        }
    }
}
