using System.Linq;
using System.Net;
using System.Net.Http;
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
        [FunctionName("PostUser")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var message = req.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<User>(message);

            return req.CreateResponse(HttpStatusCode.OK, $"You created a user called {user.FirstName} {user.LastName}");
        }
    }
}
