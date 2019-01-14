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
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace BeginningAzureServerlessArchitecture
{
    public static class PostTransactions
    {

        [FunctionName("PostTransactions")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Transactions")]HttpRequestMessage req,
            [DocumentDB(ConnectionStringSetting = "CosmosDBConnectionString")] DocumentClient client,
            [DocumentDB("AzureServerlessArchitectureCourse","transactions", ConnectionStringSetting = "CosmosDBConnectionString")] out Transaction transaction,
            ILogger log)
        {
            // ILogger allows you to do structured logs. An information log would simply inform of an event occurring, a warning log would flag potential issues and a critical log would record a serious problem
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            
            var message = req.Content.ReadAsStringAsync().Result;

            log.LogDebug($"Message received: {0}",message);

            Transaction transactionBeforePasswordVerification;

            try
            {
                transactionBeforePasswordVerification = JsonConvert.DeserializeObject<Transaction>(message);
            }
            catch(Exception e)
            {
                log.LogError("No valid object received, Message was: {0}", message);

                transaction = null;
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "The request did not match the required schema");
            }

            // Using the Document DB SDK directly. Setting MaxItemCount to -1 means return all results
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // This is using the LINQ API. It's possible to do .Where(b => b....) on the end of here.
            IQueryable<User> userQuery =
                client.CreateDocumentQuery<User>(
                    UriFactory.CreateDocumentCollectionUri("AzureServerlessArchitectureCourse", "users"), queryOptions).Where(a => a.EmailAddress == transactionBeforePasswordVerification.UserEmail);

            // Setup the response object
            var users = new List<User>();

            // Now execute
            foreach (User user in userQuery)
            {
                users.Add(user);
            }

            if (users.Count != 1)
            {
                transaction = null;
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Your Username or Password do not match our records");
            }

            var user = users.First();

            // find matching password hash
            byte[] savedPasswordHash = Convert.FromBase64String(user.Password);
            byte[] salt = new byte[16];
            Array.Copy(savedPasswordHash, 0, salt, 0, 16);

            // hash entered password
            var pbkdf2 = new Rfc2898DeriveBytes(transactionBeforePasswordVerification.UserPassword, salt, 10000);
            byte[] newPasswordHash = pbkdf2.GetBytes(20);

            for(int i=0; i < 20; i++)
            {
                if(savedPasswordHash[i+16] != newPasswordHash[i])
                {
                    transaction = null;
                    return req.CreateErrorResponse(HttpStatusCode.BadRequest, "Your Username or Password do not match our records");
                }
            }
            transactionBeforePasswordVerification.UserEmail = null;
            transactionBeforePasswordVerification.UserPassword = null;

            transaction = transactionBeforePasswordVerification;

            log.LogInformation("Request successful");
            return req.CreateResponse(HttpStatusCode.OK, $"You made a transaction of £{transaction.Amount}!");
            
        }
        
    }
}
