using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class LocationsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");
        private readonly HttpClient _client;
        private readonly Locations locations1 = new Locations();

        public LocationsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        public IActionResult CreatePartial()
        {
            return PartialView("Create", locations1);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Locations> locations = new List<Locations>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "GetLocations");

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Locations>>(result);
                if (data != null)
                {
                    locations = data;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }
            
            return View(locations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Locations model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "AddLocations", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }
            else // If ModelState is invalid
            {
                TempData["ErrorMessage"] = "Invalid model state"; // Store error message in TempData
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult DetailsPartial(int locationId)
        {
            return PartialView("Details", locations1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Locations model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + "EditLocations/" + model.Id, content);  // Adjust the endpoint to match your API

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
        public async Task<ActionResult<Locations>> Details(int id)
        {
            try
            {
                Locations locations = new Locations();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"GetLocationsById/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(result))
                    {
                        return RedirectToAction("Index"); // Handle no content found
                    }

                    var data = JsonConvert.DeserializeObject<Locations>(result);
                    if (data != null)
                    {
                        locations = data;
                    }
                    return locations;
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
        public IActionResult EditPartial()
        {
            var locations1 = new Locations();
            return PartialView("Edit", locations1);
        }

    }
}
