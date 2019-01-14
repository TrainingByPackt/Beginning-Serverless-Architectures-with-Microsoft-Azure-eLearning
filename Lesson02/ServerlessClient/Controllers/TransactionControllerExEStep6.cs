using Newtonsoft.Json;
using ServerlessClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ServerlessClient.Controllers
{
    public class TransactionController : Controller
    {
        private HttpClient http = new HttpClient();
        private string key = "Your Function Key";
        private string functionAddress = "www.YourFunctionAddress.com";
        // GET: Transaction
        public ActionResult Index()
        {
            // Add your key as a query parameter.
            var builder = new UriBuilder(functionAddress);
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["code"] = key;
            builder.Query = query.ToString();

            var response = http.GetAsync(builder.ToString());
            var result = response.Result.Content.ReadAsStringAsync().Result;
            var transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);

            // Useful code for debugging front end without connecting to function
            /*
                var transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Amount = 22.3m,
                        ExecutionTime = new DateTime()
                    }
                };
            */
            return View(transactions);
        }

        
        // GET: Transaction/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Transaction/Create
        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            var stringifiedTransaction = JsonConvert.SerializeObject(transaction);
            var content = new StringContent(stringifiedTransaction, Encoding.UTF8, "application/json");
            var result = http.PostAsync(functionAddress, content);
            // Usually do some error handling here. 

            return View();
        }

        // GET: Transaction/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Transaction/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Transaction/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Transaction/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
