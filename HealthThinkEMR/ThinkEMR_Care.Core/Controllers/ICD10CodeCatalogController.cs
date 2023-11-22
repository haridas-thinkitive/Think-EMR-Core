using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ThinkEMR_Care.Core.Models.MasterDashboard;

namespace ThinkEMR_Care.Core.Controllers
{
    public class ICD10CodeCatalogController : Controller
    {
        private Uri baseUrl = new Uri("https://localhost:7286/");
        private readonly HttpClient _client;
        private readonly ICD10CodeCatalog icD10CodeCatalogs = new ICD10CodeCatalog();

        public ICD10CodeCatalogController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseUrl;
        }
        public IActionResult Create()
        {
            return PartialView("CreateICD10", icD10CodeCatalogs);
        }

        [HttpGet]
        public IActionResult ICD10CodeCatalogIndex()
        {
            List<ICD10CodeCatalog> icdCatalogList = new List<ICD10CodeCatalog>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "GetAllICD10CodeCatalog").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ICD10CodeCatalog>>(result);
                if (data != null)
                {
                    icdCatalogList = data;
                }
            }
            return View(icdCatalogList);
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<ICD10CodeCatalog> icd10CodeCatalogList = new List<ICD10CodeCatalog>();
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
                var data = JsonConvert.DeserializeObject<List<ICD10CodeCatalog>>(result);

                if (data != null)
                {
                    icd10CodeCatalogList = data;
                }
            }
            return View(icd10CodeCatalogList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateICD10Catalog(ICD10CodeCatalog model)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _client.PostAsync("ICD10CodeCatalog/CreateICD10CodeCatalog", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ICD10CodeCatalogIndex");
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
