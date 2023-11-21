using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class UsersController : Controller
    {

        Uri baseAddress = new Uri("https://localhost:7286");
        private readonly HttpClient _client;
        private readonly StaffUser staffUser = new StaffUser();

        public UsersController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<StaffUser> staffUsers = new List<StaffUser>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "GetStaffUsers");

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<StaffUser>>(result);
                if (data != null)
                {
                    staffUsers = data;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }

            return View(staffUsers);
        }

        public IActionResult CreatePartial()
        {
            return PartialView("Create", staffUser);
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StaffUser model)
        {
            if (ModelState.IsValid)
            {
                
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "AddStaffUsers", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"API request failed with status code {response.StatusCode}. Content: {await response.Content.ReadAsStringAsync()}");
                }
            }
            return View(model);
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(StaffUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PostAsync("AddStaffUsers", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // Handle validation errors if necessary
                        var validationErrors = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"API request failed with status code {response.StatusCode}. Validation errors: {validationErrors}");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"API request failed with status code {response.StatusCode}. Content: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }
            return View(model);
        }


        [HttpGet]
        public async Task<ActionResult<StaffUser>> Edit(int id)
        {
            try
            {
                StaffUser user = new StaffUser();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"GetStaffUsersById/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(result))
                    {
                        return RedirectToAction("Index"); // Handle no content found
                    }
                    var data = JsonConvert.DeserializeObject<StaffUser>(result);
                    if (data != null)
                    {
                        user = data;
                    }
                    return user;
                }
                else if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    return RedirectToAction("Index"); // Handle no content found
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StaffUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + $"EditStaffUsers/" + model.Id, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "API request failed");
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return View(model);
        }

        public IActionResult DetailsPartial(int locationId)
        {
            return PartialView("Edit", staffUser);
        }

        /// <summary>
        /// Provider User
        /// </summary>
        /// <returns></returns>

        

        public async Task<ActionResult> AddProvider(ProviderUser model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PostAsync("AddProviderUsers", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // Handle validation errors if necessary
                        var validationErrors = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, $"API request failed with status code {response.StatusCode}. Validation errors: {validationErrors}");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"API request failed with status code {response.StatusCode}. Content: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
                }
            }
            return View(model);
        }

    }
}
