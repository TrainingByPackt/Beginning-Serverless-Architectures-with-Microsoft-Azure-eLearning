﻿using Newtonsoft.Json;
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
    public class UserController : Controller
    {
        private HttpClient http = new HttpClient();
        private string key = "Your Function Key";
        // GET: Transaction
        public ActionResult Index()
        {
            // Add your key as a query parameter.
            var builder = new UriBuilder("Your Function Address here");
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["code"] = key;
            builder.Query = query.ToString();

            var url = builder.ToString();
            var response = http.GetAsync(url);
            var result = response.Result.Content.ReadAsStringAsync().Result;
            var users = JsonConvert.DeserializeObject<List<User>>(result);

            // Useful code for debugging front end
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
            return View(users);
        }


        // GET: Transaction/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            // Add your key as a query parameter.
            var builder = new UriBuilder("Your Function Address here");
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["code"] = key;
            builder.Query = query.ToString();

            var url = builder.ToString();
            var stringifiedUser = JsonConvert.SerializeObject(user);
            var content = new StringContent(stringifiedUser, Encoding.UTF8, "application/json");
            var result = http.PostAsync(url, content).Result;
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
