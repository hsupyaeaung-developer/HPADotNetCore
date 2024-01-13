using Azure;
using HPADotNetCore.MvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace HPADotNetCore.MvcApp.Controllers
{
    public class BlogHttpClientController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public BlogHttpClientController(HttpClient httpClient, IConfiguration configuration, AppDbContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            BlogListResponseModel model = new BlogListResponseModel();
            HttpResponseMessage response = await _httpClient.GetAsync("api/blog");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonString)!;
            }
            return View("~/Views/BlogRefit/BlogIndex.cshtml", model);
        }

        [ActionName("List")]
        public async Task<IActionResult> BlogList(int pageNo = 1, int pageSize = 10)
        {
            var responseModel = await _httpClient.GetAsync($"api/blog/{pageNo}/{pageSize}");
            BlogListResponseModel model = new BlogListResponseModel();
            if (responseModel.IsSuccessStatusCode)
            {
                string jsonString = await responseModel.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonString)!;
            }
            BlogDataResponseModel dataModel = new BlogDataResponseModel();
            int rowCount = await _context.Blogs.CountAsync();
            int pageCount = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                pageCount++;

            dataModel.Blogs = model.Data;
            dataModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, "/bloghttpclient/list");

            return View("BlogList", dataModel);
        }

        [ActionName("Create")]
        public IActionResult BlogCreate()
        {
            return View("BlogCreate");
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> BlogSave(BlogDataModel reqModel)
        {
            string blogJson = JsonConvert.SerializeObject(reqModel);
            BlogListResponseModel model = new BlogListResponseModel();
            HttpContent httpContent = new StringContent(blogJson, Encoding.UTF8, Application.Json);
            HttpClient client = new HttpClient();

            var response = await _httpClient.PostAsync("api/blog", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonString)!;
            }
            string message = model.Message;
            TempData["Message"] = message;
            TempData["IsSuccess"] = model.IsSuccess;

            MessageModel msgModel = new MessageModel(model.IsSuccess, message);
            return Json(msgModel);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> BlogEdit(int id)
        {
            var responseModel = await _httpClient.GetAsync($"api/blog/{id}");
            BlogResponseModel model = new BlogResponseModel();
            BlogDataModel dataModel = new BlogDataModel();
            if (responseModel.IsSuccessStatusCode)
            {
                string jsonString = await responseModel.Content.ReadAsStringAsync();

                model = JsonConvert.DeserializeObject<BlogResponseModel>(jsonString)!;
                if (model.IsSuccess)
                {
                    dataModel = new BlogDataModel
                    {
                        Blog_Id = model.Data.Blog_Id,
                        Blog_Author = model.Data.Blog_Author,
                        Blog_Content = model.Data.Blog_Content,
                        Blog_Title = model.Data.Blog_Title
                    };
                }
                else
                {
                    TempData["Message"] = "No data found.";
                    TempData["IsSuccess"] = false;
                    return Redirect("/bloghttpclient/list");
                }
            }
            return View("BlogEdit", dataModel);
        }

        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> BlogUpdate(int id, BlogDataModel reqModel)
        {
            string blogJson = JsonConvert.SerializeObject(reqModel);
            HttpContent httpContent = new StringContent(blogJson, Encoding.UTF8, Application.Json);
            var responseModel = await _httpClient.PatchAsync($"api/blog/{id}", httpContent);
            BlogResponseModel model = new BlogResponseModel();
            if (responseModel.IsSuccessStatusCode)
            {
                string jsonString = await responseModel.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<BlogResponseModel>(jsonString)!;
            }

            string message = model.Message;
            TempData["Message"] = message;
            TempData["IsSuccess"] = model.IsSuccess;
            MessageModel msgModel = new MessageModel(model.IsSuccess, message);
            return Json(msgModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> BlogDelete(BlogDataModel reqModel)
        {
            var responseModel = await _httpClient.DeleteAsync($"api/blog/{reqModel.Blog_Id}");
            BlogResponseModel model = new BlogResponseModel();
            if (responseModel.IsSuccessStatusCode)
            {
                string jsonString = await responseModel.Content.ReadAsStringAsync();

                model = JsonConvert.DeserializeObject<BlogResponseModel>(jsonString)!;
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
            string message = model.Message;
            TempData["Message"] = message;
            TempData["IsSuccess"] = model.IsSuccess;

            MessageModel msgModel = new MessageModel(model.IsSuccess, message);
            return Json(msgModel);
        }
    }
}
