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
            if(HttpContext.Session.GetString("JWToken") != null)
            {
                return RedirectToAction("Index", "AdminDashboard");
            }
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

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword (EmailModel emailmodel)
        {
            if(!ModelState.IsValid)
            {
                TempData["errormessage"] = "Please enter a valid email address.";
                return View(emailmodel);
            }
            else
            {
                try
                {
                    using (var httpClient = new HttpClient())
                    {
                        StringContent stringContent = new StringContent(JsonConvert.SerializeObject(emailmodel), Encoding.UTF8, "application/json");
                        string apiUrl = $"https://localhost:7286/api/AuthenticationService/ForgotPassword?email={Uri.EscapeDataString(emailmodel.Email)}";

                        using (var response = await httpClient.PostAsync(apiUrl, stringContent))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string ResetPassResponce = HttpContext.Session.GetString("JWToken");
                                string responseContent = await response.Content.ReadAsStringAsync();
                                var apiResponse = JsonConvert.DeserializeObject<PasswordResetResponse>(responseContent);

                                PasswordResetResponse apiResponseData = new PasswordResetResponse
                                {
                                    Status = apiResponse.Status,
                                    Otp = apiResponse.Otp,
                                    Token = apiResponse.Token,
                                    Email = apiResponse.Email,
                                    Message = apiResponse.Message
                                };
                                string apiResponseJson = JsonConvert.SerializeObject(apiResponseData);
                                HttpContext.Session.SetString("ApiResponse", apiResponseJson);



                                if (apiResponse.Status)
                                {
                                    HttpContext.Session.SetString("ResetPassword", apiResponse.Token);
                                    return RedirectToAction("EnterOTP", "Account");
                                }
                                else
                                {
                                    TempData["errormessage"] = "Incorrect Email";
                                }
                            }
                            else
                            {
                                TempData["errormessage"] = "An error occurred while processing the login request.";
                            }
                            return RedirectToAction("Login", "Account");
                        }

                    }
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }
        }

        
        public async Task<IActionResult> EnterOTP()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnterOTP([FromBody] UserOTP userOTP)
        {
            if(!ModelState.IsValid)
            {
                TempData["errormessage"] = "Please check the provided information.";
                return Json(new { redirectTo = "Error" });
            }
            else
            {
                try
                {
                    var UserOTPData = userOTP.InputOTP;
                    if (!string.IsNullOrEmpty(UserOTPData))
                    {
                        string apiResponseJsonExistSession = HttpContext.Session.GetString("ApiResponse");
                        string ResendOTPSessionCreate = HttpContext.Session.GetString("ResendOTP");

                        if (!string.IsNullOrEmpty(ResendOTPSessionCreate) )
                        {
                            PasswordResetResponse apiResponse = JsonConvert.DeserializeObject<PasswordResetResponse>(ResendOTPSessionCreate);

                            if (apiResponse != null)
                            {
                                if (apiResponse.Otp == userOTP.InputOTP)
                                {
                                    HttpContext.Session.Remove("ApiResponse");
                                    return Json(new { redirectTo = "ResetPassword" });
                                }
                                else
                                {
                                    HttpContext.Session.Remove("ApiResponse");
                                    TempData["errormessage"] = "OTP did not match.";
                                    return Json(new { redirectTo = "Error" });
                                }
                            }
                        }else if(!string.IsNullOrEmpty(apiResponseJsonExistSession))
                        {
                            PasswordResetResponse apiResponseExistSession = JsonConvert.DeserializeObject<PasswordResetResponse>(apiResponseJsonExistSession);

                            if (apiResponseExistSession != null)
                            {
                                if (apiResponseExistSession.Otp == userOTP.InputOTP)
                                {
                                    HttpContext.Session.Remove("ResendOTP");
                                    return Json(new { redirectTo = "ResetPassword" });
                                }
                                else
                                {
                                    HttpContext.Session.Remove("ResendOTP");
                                    TempData["errormessage"] = "OTP did not match.";
                                    return Json(new { redirectTo = "Error" });
                                }
                            }
                        }
                    }
                    TempData["errormessage"] = "Please Try Again..!!";
                    return View(userOTP);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public async Task<IActionResult> ResetPassword()
        {
            return  View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordData resetPassword)
        {
            try
            {
                if (resetPassword.Password != null && resetPassword.ConfirmPassword != null && resetPassword.Password == resetPassword.ConfirmPassword)
                {
                    string ExistingSessionOTP = HttpContext.Session.GetString("ApiResponse");
                    string ResendSessionOTP = HttpContext.Session.GetString("ResendOTP");

                    if (!string.IsNullOrEmpty(ExistingSessionOTP))
                    {
                        ResetPasswordData apiResponse = JsonConvert.DeserializeObject<ResetPasswordData>(ExistingSessionOTP);
                        if (apiResponse != null)
                        {
                            var RequestBody = new ResetPasswordData
                            {
                                Password = resetPassword.Password,
                                ConfirmPassword = resetPassword.ConfirmPassword,
                                Email = apiResponse.Email,
                                Token = apiResponse.Token,
                                OTP = apiResponse.OTP,
                            };

                            using (var httpClient = new HttpClient())
                            {
                                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(RequestBody), Encoding.UTF8, "application/json");
                                using (var response = await httpClient.PostAsync("https://localhost:7286/api/AuthenticationService/Reset-Password", stringContent))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        HttpContext.Session.Remove("ApiResponse");
                                        TempData["Success"] = "Password Updated Successfully";
                                        return RedirectToAction("Login", "Account");
                                    }
                                    else
                                    {
                                        TempData["errormessage"] = "Something went wrong with the API request.";
                                    }
                                }
                            }
                        }
                        else
                        {
                            TempData["errormessage"] = "Invalid API response data";
                        }
                    } else if(!string.IsNullOrEmpty(ResendSessionOTP))
                    {
                        ResetPasswordData apiResponse = JsonConvert.DeserializeObject<ResetPasswordData>(ResendSessionOTP);
                        if (apiResponse != null)
                        {
                            var RequestBody = new ResetPasswordData
                            {
                                Password = resetPassword.Password,
                                ConfirmPassword = resetPassword.ConfirmPassword,
                                Email = apiResponse.Email,
                                Token = apiResponse.Token,
                                OTP = apiResponse.OTP,
                            };

                            using (var httpClient = new HttpClient())
                            {
                                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(RequestBody), Encoding.UTF8, "application/json");
                                using (var response = await httpClient.PostAsync("https://localhost:7286/api/AuthenticationService/Reset-Password", stringContent))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        HttpContext.Session.Remove("ResendOTP");
                                        TempData["Success"] = "Password Updated Successfully";
                                        return RedirectToAction("Login", "Account");
                                    }
                                    else
                                    {
                                        TempData["errormessage"] = "Something went wrong with the API request.";
                                    }
                                }
                            }
                        }
                        else
                        {
                            TempData["errormessage"] = "Invalid API response data";
                        }
                    }
                    else
                    {
                        TempData["errormessage"] = "Password and ConfirmPassword do not match";
                    }
                }
                else
                {
                    TempData["errormessage"] = "Password and ConfirmPassword should be same";
                    return View(resetPassword);
                }

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResendOTP()
        {
            try
            {
                string apiResponseJson = HttpContext.Session.GetString("ApiResponse");

                if (string.IsNullOrEmpty(apiResponseJson))
                {
                    return RedirectToAction("Login", "Account");
                }
           
                PasswordResetResponse apiResponse = JsonConvert.DeserializeObject<PasswordResetResponse>(apiResponseJson);

                using (var httpClient = new HttpClient())
                {
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(apiResponse), Encoding.UTF8, "application/json");
                    string apiUrl = $"https://localhost:7286/api/AuthenticationService/ResendOTP?email={Uri.EscapeDataString(apiResponse.Email)}";

                    using (var response = await httpClient.PostAsync(apiUrl, stringContent))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string ResendOTPSessionCreate = HttpContext.Session.GetString("ResendOTP");
                            string responseContent = await response.Content.ReadAsStringAsync();
                            var ResendOTP = JsonConvert.DeserializeObject<ResendOTPSessionData>(responseContent);

                            HttpContext.Session.SetString("ResendOTP", JsonConvert.SerializeObject(new
                            {
                                ResendOTP.Status,
                                ResendOTP.Otp,
                                ResendOTP.Token,
                                ResendOTP.Email,
                                ResendOTP.Message
                            }));

                            return RedirectToAction("EnterOTP", "Account");
                        }
                    }
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
