using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ThinkEMR_Care.Core.Models.MasterDashboard;

namespace ThinkEMR_Care.Core.Controllers
{
    public class DrugCatalogController : Controller
    {
        private Uri baseUrl = new Uri("https://localhost:7286/");
        private readonly HttpClient _client;
        private readonly DrugCatalog drugCatalogs = new DrugCatalog();

        public DrugCatalogController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseUrl;

        }
        public IActionResult Create()
        {
            return PartialView("CreateDrugCatalog", drugCatalogs);
        }
        public IActionResult Find()
        {
            return PartialView("FindById", drugCatalogs);
        }
        //public IActionResult Find1()
        //{
        //    return PartialView("FindById", drugCatalogs);
        //}

        //[HttpGet]
        //public async Task<IActionResult> DrugCatalogIndex(string drugSerach)
        //{
        //    ViewData["GetDrugDetails"] = drugSerach;

        //    var dataquery = from x in 
        //}

        [HttpGet]
        public IActionResult DrugCatalogIndex()
        {
            List<DrugCatalog> drugCatalogList = new List<DrugCatalog>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "GetAllDrugCatalog").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<DrugCatalog>>(result);
                if (data != null)
                {
                    drugCatalogList = data;
                }
            }
            return View(drugCatalogList);
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<DrugCatalog> adminDashboardList = new List<DrugCatalog>();
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
                var data = JsonConvert.DeserializeObject<List<DrugCatalog>>(result);

                if (data != null)
                {
                    adminDashboardList = data;
                }
            }
            return View(adminDashboardList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDrugCatalog(DrugCatalog model)
        {
            if (ModelState.IsValid)
            {

                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                HttpResponseMessage response = await _client.PostAsync("DrugCatalog/CreateDrugCatalog", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("DrugCatalogIndex");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult<DrugCatalog> FindById(int id)
        {
            DrugCatalog drugCatalog = new DrugCatalog();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "FindById/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<DrugCatalog>(result);
                if (data != null)
                {
                    drugCatalog = data;
                }
            }
            return drugCatalog;

            // return View(drugCatalog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FindById(DrugCatalog model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress +"DrugCatalog/UpdateDrugCatalog/" + model.Id, content);
                //  HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "GetAllDrugCatalog").Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("DrugCatalogIndex");
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
