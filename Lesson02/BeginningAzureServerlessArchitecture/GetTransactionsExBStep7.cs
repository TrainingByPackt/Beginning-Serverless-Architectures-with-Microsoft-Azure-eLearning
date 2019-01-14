using System.Collections.Generic;
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
        public static List<Transaction> Run(
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

            // Using LINQ. It's possible to do .Where(a => a.property = "QueryString") on the end of here
            IQueryable<Transaction> transactionQuery =
                client.CreateDocumentQuery<Transaction>(
                    UriFactory.CreateDocumentCollectionUri("AzureServerlessArchitectureCourse","Transactions"), queryOptions);

            // Initialize the response object
            var resultList = new List<Transaction>();

            // Now execute the query and add results to response list.
            foreach(Transaction transaction in transactionQuery)
            {
                resultList.Add(transaction);
            }

            return resultList;
        }
    }
}
