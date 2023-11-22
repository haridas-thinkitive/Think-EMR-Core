using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ThinkEMR_Care.Core.Models;
using ThinkEMR_Care.Core.Models.MasterDashboard;

namespace ThinkEMR_Care.Core.Controllers
{
    public class CPTCodeCatalogController : Controller
    {
        private Uri baseUrl = new Uri("https://localhost:7286/");
        private readonly HttpClient _client;
        private readonly CPTCodeCatalog cPTCodeCatalog = new CPTCodeCatalog();

        public CPTCodeCatalogController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseUrl;
        }
        public IActionResult Create()
        {
            return PartialView("CreateCPT",cPTCodeCatalog);
        }

        [HttpGet]
        public IActionResult CPTCodeCatalogIndex()
        {
            string jwtToken = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            List<CPTCodeCatalog> cptCatalogList = new List<CPTCodeCatalog>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "GetAllCPCTCodeCatalog").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<CPTCodeCatalog>>(result);
                if (data != null)
                {
                    cptCatalogList = data;
                }
            }
            return View(cptCatalogList);
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<CPTCodeCatalog> cptCodeCatalogList = new List<CPTCodeCatalog>();
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
                var data = JsonConvert.DeserializeObject<List<CPTCodeCatalog>>(result);

                if (data != null)
                {
                    cptCodeCatalogList = data;
                }
            }
            return View(cptCodeCatalogList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCPT(CPTCodeCatalog model)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _client.PostAsync("CPCTCodeCatalog/CreateCPCTCodeCatalog", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("CPTCodeCatalogIndex");
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
