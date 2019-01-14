using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BeginningAzureServerlessArchitecture.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace BeginningAzureServerlessArchitecture
{
    public static class PostUsers
    {
        [FunctionName("PostUsers")]
        public static HttpResponseMessage Run(
        [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "users")] HttpRequestMessage req,
        [DocumentDB(
                "AzureServerlessArchitectureCourse",
                "Transactions",
                ConnectionStringSetting = "CosmosDBConnectionString"
            )] out User user,
        TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var message = req.Content.ReadAsStringAsync().Result;
            var userPrePasswordHash = JsonConvert.DeserializeObject<User>(message);

            // Hash the password
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(userPrePasswordHash.Password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            userPrePasswordHash.Password = Convert.ToBase64String(hashBytes);
            user = userPrePasswordHash;

            return req.CreateResponse(HttpStatusCode.OK, $"You created a user called {user.FirstName} {user.LastName}");
        }
    }

}
