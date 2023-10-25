using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if(ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync("https://localhost:7286/api/Usermanagement/Login", stringContent))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string responseContent = await response.Content.ReadAsStringAsync();
                            var apiResponse = JsonConvert.DeserializeObject<APIResponce<string>>(responseContent);

                            if (apiResponse.IsSuccess)
                            {
                                HttpContext.Session.SetString("JWToken", apiResponse.Responce);

                                return RedirectToAction("Index", "AdminDashboard");
                            }
                            else
                            {
                                TempData["errormessage"]= "Incorrect Email and Password";
                            }
                        }
                        else
                        {
                            ViewBag.Message = "An error occurred while processing the login request.";
                        }
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
