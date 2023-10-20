using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class ProviderGroupsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");// Base URL of your API
        private readonly HttpClient _client;
        private static ProviderGroupProfile pgprofiles = new ProviderGroupProfile();

        public ProviderGroupsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        public IActionResult CreatePartial()
        {
            return PartialView("Create", pgprofiles);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<ProviderGroupProfile> providerGroups = new List<ProviderGroupProfile>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "GetProviderGroups");

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ProviderGroupProfile>>(result);
                if (data != null)
                {
                    providerGroups = data;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }
            
            return View(providerGroups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProviderGroupProfile model)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "AddProviderGroups", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); 
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }

            return View(model); 
        }


    }
}
