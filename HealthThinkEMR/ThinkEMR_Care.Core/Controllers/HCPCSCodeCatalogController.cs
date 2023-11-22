using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ThinkEMR_Care.Core.Models.MasterDashboard;

namespace ThinkEMR_Care.Core.Controllers
{
    public class HCPCSCodeCatalogController : Controller
    {
        private Uri baseUrl = new Uri("https://localhost:7286/");
        private readonly HttpClient _client;
        private readonly HCPCSCodeCatalog hPCSCodeCatalogs = new HCPCSCodeCatalog();
        public HCPCSCodeCatalogController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseUrl;
        }
        public IActionResult Create()
        {
            return PartialView("CreateHCPCS", hPCSCodeCatalogs);
        }
        //public IActionResult MasterPartial()
        //{
        //    return PartialView("AllButtons");
        //}
        [HttpGet]
        public IActionResult HCPCSCodeCatalogIndex()
        {
            List<HCPCSCodeCatalog> hcpcsCatalogList = new List<HCPCSCodeCatalog>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "GetAllHCPCSCodeCatalog").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HCPCSCodeCatalog>>(result);
                if (data != null)
                {
                    hcpcsCatalogList = data;
                }
            }
            return View(hcpcsCatalogList);
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<HCPCSCodeCatalog> hcpcsCodeCatalogList = new List<HCPCSCodeCatalog>();
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
                var data = JsonConvert.DeserializeObject<List<HCPCSCodeCatalog>>(result);

                if (data != null)
                {
                    hcpcsCodeCatalogList = data;
                }
            }
            return View(hcpcsCodeCatalogList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateHCPCSCatalog(HCPCSCodeCatalog model)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _client.PostAsync("HCPCSCodeCatalog/CreateHCPCSCodeCatalog", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("HCPCSCodeCatalogIndex");
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
