using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ThinkEMR_Care.Core.Models;
using ThinkEMR_Care.Core.ViewModels;

namespace ThinkEMR_Care.Core.Controllers
{
    public class LocationsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");
        private readonly HttpClient _client;
        

        public LocationsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Locations> locations = new List<Locations>();
            List<ProviderGroupProfile> providers = new List<ProviderGroupProfile>();
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
            var viewModel = new LocationViewModel
            {
                Locations = locations,
                ProviderGroupProfiles = providers
            };

            return View(viewModel);
        }
    }
}
