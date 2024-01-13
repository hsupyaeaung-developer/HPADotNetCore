using HPADotNetCore.MvcApp.Interfaces;
using HPADotNetCore.MvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPADotNetCore.MvcApp.Controllers
{
    public class BlogRefitController : Controller
    {
        private readonly IBlogApi _blogApi;
        private readonly AppDbContext _context;

        public BlogRefitController(IBlogApi blogApi, AppDbContext context)
        {
            _blogApi = blogApi;
            _context = context;
        }

        [ActionName("Index")]
        public async Task<IActionResult> BlogIndex()
        {
            var model = await _blogApi.GetBlogs();
            return View("BlogIndex", model);
        }

        [ActionName("List")]
        public async Task<IActionResult> BlogList(int pageNo = 1, int pageSize = 10)
        {
            var responseModel = await _blogApi.GetBlogs(pageNo, pageSize);
            BlogDataResponseModel model = new BlogDataResponseModel();
            int rowCount = await _context.Blogs.CountAsync();
            int pageCount = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                pageCount++;

            model.Blogs = responseModel.Data;
            model.PageSetting = new PageSettingModel(pageNo, pageSize, pageCount, "/blogrefit/list");

            return View("BlogList", model);
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
            var responseModel = await _blogApi.CreateBlog(reqModel);
            string message = responseModel.Message;
            TempData["Message"] = message;
            TempData["IsSuccess"] = responseModel.IsSuccess;

            MessageModel model = new MessageModel(responseModel.IsSuccess, message);
            return Json(model);
        }
        
        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> BlogEdit(int id)
        {
            var responseModel = await _blogApi.GetBlog(id);
            if (responseModel is null)
            {
                TempData["Message"] = "No data found.";
                TempData["IsSuccess"] = false;
                return Redirect("/blogrefit/list");
            }
            BlogDataModel model = new BlogDataModel
            {
                Blog_Id = responseModel.Data.Blog_Id,
                Blog_Author = responseModel.Data.Blog_Author,
                Blog_Content = responseModel.Data.Blog_Content,
                Blog_Title = responseModel.Data.Blog_Title
            };
            
            return View("BlogEdit", model);
        }

        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> BlogUpdate(int id, BlogDataModel reqModel)
        {
            var responseModel = await _blogApi.UpdateBlog(id, reqModel);
            if(responseModel is not null)
            {
                string message = responseModel.Message;
                TempData["Message"] = message;
                TempData["IsSuccess"] = responseModel.IsSuccess;

                MessageModel model = new MessageModel(responseModel.IsSuccess, message);
                return Json(model);
            }
            return Json(new MessageModel(false, "No Data Found to Update"));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> BlogDelete(BlogDataModel reqModel)
        {
            var responseModel = await _blogApi.DeleteBlog(reqModel.Blog_Id);
            if (responseModel is null)
            {
                return Json(new MessageModel(false, "No data found."));
            }

            string message = responseModel.Message;
            TempData["Message"] = message;
            TempData["IsSuccess"] = responseModel.IsSuccess;

            MessageModel model = new MessageModel(responseModel.IsSuccess, message);
            return Json(model);
        }
    }
}
