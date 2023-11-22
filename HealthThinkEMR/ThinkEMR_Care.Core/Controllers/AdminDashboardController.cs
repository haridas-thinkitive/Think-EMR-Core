using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class AdminDashboardController : Controller
    {
        private string baseUrl = "https://localhost:7286/api/DashboardDetails/DashboardDetailsInfo";
        private HttpClient client=new HttpClient();

        //[HttpGet]
        //public IActionResult Index()
        //{
        //    List<AdminDashboard> adminDashboardList = new List<AdminDashboard>();
        //    HttpResponseMessage response = client.GetAsync(baseUrl).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        string result = response.Content.ReadAsStringAsync().Result;
        //        var data = JsonConvert.DeserializeObject<List<AdminDashboard>>(result);
        //        if (data != null)
        //        {
        //            adminDashboardList = data;
        //        }
        //    }
        //    return View(adminDashboardList);
        //}

        [HttpGet]
        public IActionResult Index()
        {
            List<AdminDashboard> adminDashboardList = new List<AdminDashboard>();
            string jwtToken = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(jwtToken))
            {
                return RedirectToAction("Login", "Account");
            } 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            HttpResponseMessage response = client.GetAsync(baseUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<AdminDashboard>>(result);

                if (data != null)
                {
                    adminDashboardList = data;
                }
            }
            return View(adminDashboardList);
        }


        public IActionResult Logout()
        {
             if(HttpContext.Session.GetString("JWToken") != null)
             {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                HttpContext.Session.Remove("JWToken");
                HttpContext.Session.Remove("UserName");
                return RedirectToAction("Login", "Account");
             }
            return View();
        }

    }
}
