using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ThinkEMR_Care.Core.Models.MasterDashboard;

namespace ThinkEMR_Care.Core.Controllers
{
    public class LOINCCodeCatalogController : Controller
    {
        private Uri baseUrl = new Uri("https://localhost:7286/");
        private readonly HttpClient _client;
        private readonly LOINCCodeCatalog loINCCodeCatalogs = new LOINCCodeCatalog();

        public LOINCCodeCatalogController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseUrl;
        }
        public IActionResult Create()
        {
            return PartialView("CreateLOINC", loINCCodeCatalogs);
        }
        [HttpGet]
        public IActionResult LOINCCodeCatalogIndex()
        {
            List<LOINCCodeCatalog> loincCatalogList = new List<LOINCCodeCatalog>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "GetAllLOINC").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<LOINCCodeCatalog>>(result);
                if (data != null)
                {
                    loincCatalogList = data;
                }
            }
            return View(loincCatalogList);
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<LOINCCodeCatalog> adminDashboardList = new List<LOINCCodeCatalog>();
            string jwtToken = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            HttpResponseMessage response = _client.GetAsync(baseUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<LOINCCodeCatalog>>(result);

                if (data != null)
                {
                    adminDashboardList = data;
                }
            }
            return View(adminDashboardList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLOINCCatalog(LOINCCodeCatalog model)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _client.PostAsync("LOINCCodeCatalog/CreateLOINCCodeCatalog", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("LOINCCodeCatalogIndex");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }

            return View(model);
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("JWToken") != null)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                HttpContext.Session.Remove("JWToken");
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}
