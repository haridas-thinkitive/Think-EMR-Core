using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Permissions;
using System.Text;
using ThinkEMR_Care.Core.Models.ViewModel;
using ThinkEMR_Care.DataAccess.Models.Roles_and_Responsibility;

namespace ThinkEMR_Care.Core.Controllers
{
    public class RolesAndResponsibilityController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7286");// Base URL of your API
        private readonly HttpClient _client;
        /*private static RolePermission rolePermission = new RolePermission();
        private static RoleUser role = new RoleUser();*/

        public RolesAndResponsibilityController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            List<RolePermission> rolePermission = new List<RolePermission>();

            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "GetRolesAndResponsibility");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<RolePermission>>(result);
                if (data != null)
                {
                    rolePermission = data;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "API request failed");
                }
            }

            return View(rolePermission);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleUserVM model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "AddNewRole", content);

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



        public async Task<IActionResult> CreatePartial()
        {
            var roleTypes = await FetchDataFromApi<List<RoleTypes>>("GetRoleTypes");
            var permissions = await FetchDataFromApi<List<Permission>>("GetPermissions");

            var viewModel = new RoleUserViewModel
            {
                RoleTypes = new SelectList(roleTypes, "RoleTypeId", "RoleTypeName"),
                Permissions = permissions,
                RoleUser = new RoleUser()
            };

            return PartialView("Create", viewModel);
        }

        private async Task<T> FetchDataFromApi<T>(string apiEndpoint) where T : class, new()
        {
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + apiEndpoint);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(result);
            }

            return new T(); // Return an empty object of type T
        }


    }
}
