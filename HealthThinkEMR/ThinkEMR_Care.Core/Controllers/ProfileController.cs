using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class ProfileController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");// Base URL of your API
        private readonly HttpClient _client;
        private static ProviderGroupProfile pgprofiles = new ProviderGroupProfile();

        public ProfileController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            ProviderGroupProfile profile = new ProviderGroupProfile();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"GetProviderGroupsById/{id}");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ProviderGroupProfile>(result);
                if(data != null)
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

        public IActionResult ProfilePartial()
        {
            return PartialView("_Profile", pgprofiles);
        }

    }
}
