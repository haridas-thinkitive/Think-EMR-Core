using Microsoft.AspNetCore.Mvc;

namespace ThinkEMR_Care.Core.Controllers
{
    public class PatientsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");
        private readonly HttpClient _client;

        public PatientsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
          
        public IActionResult Index()
        {
            return View();
        }
    }
}
