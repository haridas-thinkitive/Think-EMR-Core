using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class DepartmentsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");
        private readonly HttpClient _client;

        public DepartmentsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Departments> departments = new List<Departments>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "GetDepartments");

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Departments>>(result);
                if (data != null)
                {
                    departments = data;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }

            return View(departments);
        }
    }
}
