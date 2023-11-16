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

        //[HttpPost]
        //public async Task<IActionResult> Create(AdminProfileDetails adminProfileDetails)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            using (var httpClient = new HttpClient())
        //            {
        //                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(adminProfileDetails), Encoding.UTF8, "application/json");
        //                using (var response = await httpClient.PostAsync($"https://localhost:7286/api/AuthenticationService/Register?Role={adminProfileDetails.Role}", stringContent))
        //                {
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        string responseContent = await response.Content.ReadAsStringAsync();
        //                        var apiResponse = JsonConvert.DeserializeObject<ResponceContent>(responseContent);

        //                        if (apiResponse.status == "Success")
        //                        {
        //                            return RedirectToAction("Index", "UserSetting");
        //                        }
        //                        else
        //                        {
        //                            return RedirectToAction("Login", "Account");

        //                        }
        //                    }
        //                    return RedirectToAction("Index", "UserSetting");
        //                }
        //            }
        //        }

        //        return RedirectToAction("Login", "Account");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create([FromForm] AdminProfileDetails adminProfileDetails, IFormFile profilePhoto)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            using (var httpClient = new HttpClient())
        //            {
        //                var apiUrl = "https://localhost:7286/api/AuthenticationService/Register";
        //                var queryString = $"?Role={adminProfileDetails.Role}";
        //                var fullUrl = $"{apiUrl}{queryString}";

        //                var adminProfileJson = JsonConvert.SerializeObject(adminProfileDetails, new JsonSerializerSettings
        //                {
        //                    ContractResolver = new DefaultContractResolver
        //                    {
        //                        NamingStrategy = new CamelCaseNamingStrategy()
        //                    },
        //                    Formatting = Formatting.None,
        //                    NullValueHandling = NullValueHandling.Ignore
        //                });

        //                StringContent stringContent = new StringContent(adminProfileJson, Encoding.UTF8, "application/json");

        //                using (var response = await httpClient.PostAsync(fullUrl, stringContent))
        //                {
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        string responseContent = await response.Content.ReadAsStringAsync();
        //                        var apiResponse = JsonConvert.DeserializeObject<ResponceContent>(responseContent);

        //                        if (apiResponse.status == "Success")
        //                        {
        //                            return RedirectToAction("Index", "UserSetting");
        //                        }
        //                        else
        //                        {
        //                            return RedirectToAction("Login", "Account");
        //                        }
        //                    }
        //                    return RedirectToAction("Index", "UserSetting");
        //                }
        //            }
        //        }

        //        return RedirectToAction("Login", "Account");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AdminProfileDetails adminProfileDetails, IFormFile profilePhoto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Convert the image to a base64 string
                    string imageBase64 = await ConvertImageToBase64Async(profilePhoto);

                    // Include the base64 string in the adminProfileDetails object
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
                // Handle the exception appropriately, log it, etc.
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







    }
}
