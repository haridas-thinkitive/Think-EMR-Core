using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class AdminDashboardController : Controller
    {
        private string baseUrl = "https://localhost:7286/api/DashboardDetails/DashboardDetails";
        private HttpClient client=new HttpClient();

        [HttpGet]
        public IActionResult Index()
        {
            List<AdminDashboard> adminDashboardList = new List<AdminDashboard>();
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
    }   
}
