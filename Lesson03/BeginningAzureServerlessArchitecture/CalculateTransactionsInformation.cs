using System;
using System.Collections.Generic;
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
    public static class CalculateTransactionsInformation
    {
        [FunctionName("CalculateTransactionsInformation")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            var message = req.Content.ReadAsStringAsync().Result;

            List<Transaction> transactions = new List<Transaction>();
            try
            {
                transactions.AddRange(JsonConvert.DeserializeObject<List<Transaction>>(message));
            }
            catch (Exception e)
            {
                return req.CreateErrorResponse(HttpStatusCode.BadRequest, "The request did not match the required schema");
            }

            var totalTransactions = transactions.Sum(t => t.Amount);
            var meanTransactionSize = transactions.Average(t => t.Amount);
            var largestTransactionAmount = transactions.Max(t => t.Amount);

            var transactionsInformation = new TransactionsInformationModel
            {
                TotalTransactionsAmount = totalTransactions,
                MeanTransactionAmount = meanTransactionSize,
                LargestTransactionAmount = largestTransactionAmount
            };

            var jsonTransactionsInformation = JsonConvert.SerializeObject(transactionsInformation);

            return req.CreateResponse(HttpStatusCode.OK, jsonTransactionsInformation);
        }
    }
}
