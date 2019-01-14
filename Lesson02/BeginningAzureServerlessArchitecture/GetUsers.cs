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
    public static class GetUsers
    {
        [FunctionName("GetUsers")]
        public static List<Transaction> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users")]HttpRequestMessage req,
            [DocumentDB( ConnectionStringSetting = "CosmosDBConnectionString")] DocumentClient client,
            TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Using the Document DB SDK directly. Setting MaxItemCount to -1 means return all results
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // This is using the LINQ API. It's possible to do .Where(b => b....) on the end of here.
            IQueryable<User> userQuery = 
                client.CreateDocumentQuery<User>(
                    UriFactory.CreateDocumentCollectionUri("AzureServerlessArchitectureCourse", "Users"), queryOptions);

            // Setup the response object
            var users = new List<User>();

            // Now execute
            foreach(User user in userQuery)
            {
                users.Add(user);
            }

            // And return result
            return users;
        }
    }
}
