using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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

        public IActionResult EditPartial()
        {
            var pgprofile = new ProviderGroupProfile();
            return PartialView("Edit", pgprofile);
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
                try
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
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "API request failed: " + ex.Message);
                }
            }
            else // If ModelState is invalid
            {
                TempData["ErrorMessage"] = "Invalid model state"; // Store error message in TempData
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProviderGroupProfile model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + "EditProviderGroups/" + model.Id, content);  // Adjust the endpoint to match your API

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "API request failed: " + ex.Message);
                }
            }
            else // If ModelState is invalid
            {
                TempData["ErrorMessage"] = "Invalid model state"; // Store error message in TempData
                return RedirectToAction("Index");
            }
            return View(model);
        }

        
        [HttpGet]
        public async Task<ActionResult<ProviderGroupProfile>> GetProviderGroup(int id)
        {
            try
            {
                ProviderGroupProfile providerGroupProfile = new ProviderGroupProfile();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"GetProviderGroupsById/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(result))
                    {
                        return RedirectToAction("Index"); // Handle no content found
                    }

                    var data = JsonConvert.DeserializeObject<ProviderGroupProfile>(result);
                    if (data != null)
                    {
                        providerGroupProfile = data;
                    }
                    return providerGroupProfile;
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index"); // Handle no content found
                }
            }
            catch (Exception ex)
            {
                throw; 
            }

            return RedirectToAction("Index");
        }

    }
}
