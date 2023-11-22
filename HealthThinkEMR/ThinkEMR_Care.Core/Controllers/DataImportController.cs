using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ThinkEMR_Care.Core.Models.MasterDashboard;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ThinkEMR_Care.Core.Controllers
{
    public class DataImportController : Controller
    {
        private Uri baseUrl = new Uri("https://localhost:7286/");
        private readonly HttpClient _client;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DataImportController(IWebHostEnvironment webHostEnvironment)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseUrl;
            _webHostEnvironment = webHostEnvironment;
        }

        //[HttpPost]
        //public async Task<IActionResult> UploadFile(IFormFile file)
        //{
        //    string jwtToken = HttpContext.Session.GetString("JWToken");
        //    if (string.IsNullOrEmpty(jwtToken))
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        //    if (file != null && file.Length > 0)
        //    {

        //        string uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        //        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploaded-files");
        //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //        if (!Directory.Exists(uploadsFolder))
        //        {
        //            Directory.CreateDirectory(uploadsFolder);
        //        }

        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(fileStream);
        //        }

        //        return RedirectToAction("DataImportIndex", "DataImport");
        //    }

        //    return View();
        //}

        [HttpPost]
        public IActionResult UploadFile(IFormFile file, int id)
        {

            var fileName = $"{id}";
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploaded-files", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                
                file.CopyTo(stream);
            }

            return RedirectToAction("Index"); 
        }
        [HttpGet]
        public IActionResult DownloadFile(int id)
        {
            var fileName = $"{id}";

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploaded-files", fileName);

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                return NotFound("File not found");
            }
        }

        public IActionResult MasterPartial()
        {
            return PartialView("AllButtons");
        }

        [HttpGet]
        public IActionResult Index()
        {

            List<DataImport> adminDashboardList = new List<DataImport>();
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
                var data = JsonConvert.DeserializeObject<List<DataImport>>(result);

                if (data != null)
                {
                    adminDashboardList = data;
                }
            }
            return View(adminDashboardList);
        }
        [HttpGet]
        public IActionResult DataImportIndex()
        {
            string jwtToken = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            List<DataImport> dataimportlist = new List<DataImport>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "GetAllDataImport").Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<DataImport>>(result);
                if (data != null)
                {
                    dataimportlist = data;
                }
            }
            return View(dataimportlist);

        }
        //public IActionResult Logout()
        //{
        //    if (HttpContext.Session.GetString("JWToken") != null)
        //    {
        //        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //        HttpContext.Session.Clear();
        //        HttpContext.Session.Remove("JWToken");
        //        return RedirectToAction("Login", "Account");
        //    }
        //    return View();
        //}
    }
}
