using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Immutable;
using System.Net;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class ProfileController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");// Base URL of your API
        private readonly HttpClient _client;

        public ProfileController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {

            TempData["storedId"] = id;
            TempData.Keep("storedId");


            ProviderGroupProfile profile = new ProviderGroupProfile();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"GetProviderGroupsById/{id}");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProviderGroupProfile>(result);
                if (data != null)
                {
                    profile = data;
                }
            }

            if (profile == null)
            {
                return RedirectToAction("Index");
            }

            return View(profile);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ProfilePartial()
        {

            var value = TempData.Peek("storedId");
            ProviderGroupProfile profile = new ProviderGroupProfile();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"GetProviderGroupsById/{value}");


            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProviderGroupProfile>(result);
                if (data != null)
                {
                    profile = data;
                }
            }

            if (profile == null)
            {
                return RedirectToAction("Index");
            }

            return PartialView("_Profile", profile); // Return the fetched data as a partial view

        }
    }
}
