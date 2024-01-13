using HPADotNetCore.MvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;
using System.Text;
using System.Reflection.Metadata;

namespace HPADotNetCore.MvcApp.Controllers
{
    public class BlogRestClientController : Controller
    {
        private readonly RestClient _restClient;
        private readonly AppDbContext _context;

        public BlogRestClientController(RestClient restClient, AppDbContext context)
        {
            _restClient = restClient;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            BlogListResponseModel model = new BlogListResponseModel();
            RestRequest request = new RestRequest("api/blog", Method.Get);
            var response = await _restClient.ExecuteAsync(request);
            if(response.IsSuccessStatusCode)
            {
                string jsonString = response.Content!;
                model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonString)!;
            }

            return View("~/Views/BlogRefit/BlogIndex.cshtml", model);
        }

        [ActionName("List")]
        public async Task<IActionResult> BlogList(int pageNo = 1, int pageSize = 10)
        {
            RestRequest request = new RestRequest($"api/blog/{pageNo}/{pageSize}", Method.Get);
            var responseModel = await _restClient.ExecuteAsync(request);
            BlogListResponseModel model = new BlogListResponseModel();
            if (responseModel.IsSuccessStatusCode)
            {
                string jsonString = responseModel.Content!;
                model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonString)!;
            }
            BlogDataResponseModel dataModel = new BlogDataResponseModel();
            int rowCount = await _context.Blogs.CountAsync();
            int pageCount = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                pageCount++;

            dataModel.Blogs = model.Data;
            dataModel.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, "/blogrestclient/list");

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
            BlogListResponseModel model = new BlogListResponseModel();
            RestRequest request = new RestRequest("/api/blog", Method.Post);
            request.AddBody(reqModel);
            
            var response = await _restClient.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content!;

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
            RestRequest request = new RestRequest($"api/blog/{id}", Method.Get);
            var responseModel = await _restClient.ExecuteAsync(request);
            BlogResponseModel model = new BlogResponseModel();
            BlogDataModel dataModel = new BlogDataModel();
            if (responseModel.IsSuccessStatusCode)
            {
                string jsonString = responseModel.Content!;

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
                    return Redirect("/blogrestclient/list");
                }
            }
            return View("BlogEdit", dataModel);
        }

        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> BlogUpdate(int id, BlogDataModel reqModel)
        {
            RestRequest request = new RestRequest($"api/blog/{id}", Method.Patch);
            request.AddBody(reqModel);
            var responseModel = await _restClient.ExecuteAsync(request);
            BlogResponseModel model = new BlogResponseModel();
            if (responseModel.IsSuccessStatusCode)
            {
                string jsonString = responseModel.Content!;
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
            RestRequest request = new RestRequest($"api/blog/{reqModel.Blog_Id}", Method.Delete);
            var responseModel = await _restClient.ExecuteAsync(request);
            BlogResponseModel model = new BlogResponseModel();
            if (responseModel.IsSuccessStatusCode)
            {
                string jsonString = responseModel.Content!;

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
