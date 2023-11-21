using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class UserSettingController : Controller
    {

        private string baseUrl = "https://localhost:7286/api/AuthenticationService/GetAllAdminProfiles";
        private HttpClient client = new HttpClient();
        public IActionResult Index()
        {
            List<AdminProfileDetails> adminProfileDataList = new List<AdminProfileDetails>();
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
                var data = JsonConvert.DeserializeObject<List<AdminProfileDetails>>(result);
                if (data != null)
                {
                    adminProfileDataList = data;
                }
            }
            return View(adminProfileDataList);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AdminProfileDetails adminProfileDetails, IFormFile profilePhoto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string imageBase64 = await ConvertImageToBase64Async(profilePhoto);
                    adminProfileDetails.ProfileImage = imageBase64;
                    using (var httpClient = new HttpClient())
                    {
                        var apiUrl = "https://localhost:7286/api/AuthenticationService/Register";
                        var queryString = $"?Role={adminProfileDetails.Role}";
                        var fullUrl = $"{apiUrl}{queryString}";

                        var adminProfileJson = JsonConvert.SerializeObject(adminProfileDetails, new JsonSerializerSettings
                        {
                            ContractResolver = new DefaultContractResolver
                            {
                                NamingStrategy = new CamelCaseNamingStrategy()
                            },
                            Formatting = Formatting.None,
                            NullValueHandling = NullValueHandling.Ignore
                        });
                        var content = new StringContent(adminProfileJson, Encoding.UTF8, "application/json");
                        using (var response = await httpClient.PostAsync(fullUrl, content))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string responseContent = await response.Content.ReadAsStringAsync();
                                var apiResponse = JsonConvert.DeserializeObject<ResponceContent>(responseContent);

                                if (apiResponse.status == "Success")
                                {
                                    return RedirectToAction("Index", "UserSetting");
                                }
                                else
                                {
                                    return RedirectToAction("Login", "Account");
                                }
                            }
                            return RedirectToAction("Index", "UserSetting");
                        }
                    }
                }

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }
        }
        private async Task<string> ConvertImageToBase64Async(IFormFile image)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        [HttpGet]
        public async Task<IActionResult> AdminProfile()
        {
            AdminProfileDetails adminProfileData = null;
            string jwtToken = HttpContext.Session.GetString("JWToken");

            // Retrieve the username from the session
            string username = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(jwtToken) || string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }

            string apiUrl = $"https://localhost:7286/api/AuthenticationService/GetLoggedInUserProfile?userName={Uri.EscapeDataString(username)}";

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                adminProfileData = JsonConvert.DeserializeObject<AdminProfileDetails>(result);
                return View(adminProfileData);
            }

            return View(adminProfileData);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangeUserPassword changeUserPassword)
        {
            if (changeUserPassword.CurrentPassword == changeUserPassword.NewPassword || changeUserPassword.NewPassword != changeUserPassword.ConformNewPassword)
            {
                return Json(new { success = false, message = "Current and New Password cannot be null or empty." });
            }
                string userName = HttpContext.Session.GetString("UserName");
                string jwtToken = HttpContext.Session.GetString("JWToken");

                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(jwtToken) || changeUserPassword == null)
                {
                    return Json(new { success = false, message = "User information is invalid." });
                }
                using (var httpClient = new HttpClient())
                {
                    string apiUrl = $"https://localhost:7286/api/AuthenticationService/ChangeUserPassword?userName={Uri.EscapeDataString(userName)}&token={Uri.EscapeDataString(jwtToken)}";

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(changeUserPassword), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync(apiUrl, stringContent))
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ResponceContent>(responseContent);

                        if (response.IsSuccessStatusCode && apiResponse != null && apiResponse.status == "Success")
                        {
                            return Json(new { success = apiResponse.status });
                        }
                        return Json(new { success = false, message = apiResponse.Message });

                    }
                }
        }

    }
}
