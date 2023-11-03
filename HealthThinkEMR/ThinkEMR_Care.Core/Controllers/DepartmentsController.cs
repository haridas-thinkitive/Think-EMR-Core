using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ThinkEMR_Care.Core.Models;

namespace ThinkEMR_Care.Core.Controllers
{
    public class DepartmentsController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");
        private readonly HttpClient _client;
        private readonly Departments departments1 = new Departments();

        public DepartmentsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        public IActionResult CreatePartial()
        {
            return PartialView("Create", departments1);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<Departments> departments = new List<Departments>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "GetDepartments");

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Departments>>(result);
                if (data != null)
                {
                    departments = data;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }

            return View(departments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Departments model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "AddDepartment", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult<Departments>> Edit(int id)
        {
            try
            {
                Departments departments = new Departments();
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + $"GetDepartmentById/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(result))
                    {
                        return RedirectToAction("Index"); // Handle no content found
                    }
                    var data = JsonConvert.DeserializeObject<Departments>(result);
                    if (data != null)
                    {
                        departments = data;
                    }
                    return departments;
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
        public async Task<ActionResult> Edit(Departments model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await _client.PutAsync(_client.BaseAddress + "EditDepartment/" + model.Id, content);

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
            return PartialView("Edit", departments1);
        }
    }
}
