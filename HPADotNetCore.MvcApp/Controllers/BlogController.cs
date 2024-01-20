using HPADotNetCore.MvcApp;
using HPADotNetCore.MvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Text.Json;

namespace HPADotNetCore.MvcApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BlogController> _logger;
        public BlogController(AppDbContext context, ILogger<BlogController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [ActionName("Index")]
        public async Task<IActionResult> BlogIndex(int pageNo = 1, int pageSize = 10)
        {
            List<BlogDataModel> lst = await _context.Blogs
                .AsNoTracking()
                .OrderByDescending(x => x.Blog_Id)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int pageRowCount = await _context.Blogs.CountAsync();
            int pageCount = pageRowCount / pageSize;
            if (pageRowCount % pageSize > 0)
                pageCount++;

            BlogDataResponseModel model = new BlogDataResponseModel();
            
            model.Blogs = lst;
            model.PageSetting = new PageSettingModel
            {
                PageCount = pageCount,
                PageNo = pageNo,
                PageSize = pageSize
            };
            _logger.LogInformation("Blog Data Response Model => " + JsonSerializer.Serialize(model));
            return View("BlogIndex", model);
        }

        [ActionName("Create")]
        public IActionResult BlogCreate()
        {
            
            return View("BlogCreate");
        }

        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> BlogSave(BlogDataModel blog)
        {
            _logger.LogInformation("Blog Save Request Model => " + JsonSerializer.Serialize(blog));
            await _context.AddAsync(blog);
            var result = await _context.SaveChangesAsync();
            bool IsSuccess = result > 0;
            string Message =  result > 0 ? "Saving Successful." : "Saving Failed.";
            TempData["IsSuccess"] = IsSuccess;
            TempData["Message"] = Message;

            _logger.LogInformation("Blog Save Response Message => " + Message);
            return Redirect("/Blog");
        }

        public async Task<IActionResult> Generate()
        {
            for (int i = 1; i <= 1000; i++)
            {
                await _context.AddAsync(new BlogDataModel
                {
                    Blog_Title = i.ToString(),
                    Blog_Author = i.ToString(),
                    Blog_Content = i.ToString(),
                });
                var result = await _context.SaveChangesAsync();
            }
            return Redirect("/Blog");
        }

        [ActionName("Edit")]
        public async Task<IActionResult> BlogEdit(int id)
        {
            bool isExist = await _context.Blogs.AsNoTracking().AnyAsync(x => x.Blog_Id == id);
            if (!isExist)
            {
                TempData["IsSuccess"] = false;
                string Message = "No data found.";
                TempData["Message"] = Message;

                _logger.LogInformation("Blog Edit Resposne Message => " + Message);
                return Redirect("/Blog");
            }

            var item = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.Blog_Id == id);
            if (item == null)
            {
                TempData["IsSuccess"] = false;
                string Message = "No data found.";
                TempData["Message"] = Message;

                _logger.LogInformation("Blog Edit Response Message => " + Message);
                return Redirect("/Blog");
            }
                _logger.LogInformation("Blog Edit Response Model => " + JsonSerializer.Serialize(item));
            return View("BlogEdit", item);
        }

        [HttpPost]
        [ActionName("Update")]
        public async Task<IActionResult> BlogUpdate(int id, BlogDataModel blog)
        {
            _logger.LogInformation("Blog Update Id => " + id);
            _logger.LogInformation("Blog Update Request Model => " + JsonSerializer.Serialize(blog));

            bool isExist = await _context.Blogs.AsNoTracking().AnyAsync(x => x.Blog_Id == id);
            if (!isExist)
            {
                TempData["IsSuccess"] = false;
                string Message = "No data found.";
                TempData["Message"] = Message;

                _logger.LogInformation("Blog Update Response Message => " + Message);
                return Redirect("/Blog");
            }

            var item = await _context.Blogs.FirstOrDefaultAsync(x => x.Blog_Id == id);
            if (item == null)
            {
                TempData["IsSuccess"] = false;
                string Message = "No data found.";
                TempData["Message"] = Message;

                _logger.LogInformation("Blog Update Response Message => " + Message);
                return Redirect("/Blog");
            }

            item.Blog_Title = blog.Blog_Title;
            item.Blog_Author = blog.Blog_Author;
            item.Blog_Content = blog.Blog_Content;

            var result = await _context.SaveChangesAsync();
            TempData["IsSuccess"] = result > 0;
            string ResMessage = result > 0 ? "Updating Successful." : "Updating Failed.";
            TempData["Message"] = ResMessage;

            _logger.LogInformation("Blog Update Response Message => " + ResMessage);
            return Redirect("/Blog");
        }

        [ActionName("Delete")]
        public async Task<IActionResult> BlogDelete(int id)
        {
            _logger.LogInformation("Blog Delete Id => " + id);

            bool isExist = await _context.Blogs.AsNoTracking().AnyAsync(x => x.Blog_Id == id);
            if (!isExist)
            {
                TempData["IsSuccess"] = false;
                string Message = "No data found.";
                TempData["Message"] = Message;

                _logger.LogInformation("Blog Delete Response Message => " + Message);
                return Redirect("/Blog");
            }

            var item = await _context.Blogs.AsNoTracking().FirstOrDefaultAsync(x => x.Blog_Id == id);
            if (item == null)
            {
                TempData["IsSuccess"] = false;
                string Message = "No data found.";
                TempData["Message"] = Message;

                _logger.LogInformation("Blog Delete Response Message => " + Message);
            }

            _context.Blogs.Remove(item);
            var result = await _context.SaveChangesAsync();
            TempData["IsSuccess"] = result > 0;
            string ResMessage = result > 0 ? "Deleting Successful." : "Deleting Failed.";
            TempData["Message"] = ResMessage;

            _logger.LogInformation("Blog Delete Response Message => " + ResMessage);
            return Redirect("/Blog");
        }
    }
}
