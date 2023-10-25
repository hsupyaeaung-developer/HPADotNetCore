using HPADotNetCore.RestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HPADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBlogs()
        {
            AppDbContext db = new AppDbContext();
            
            
            var lst = db.Blogs.ToList();
            BlogListResponseModel model = new BlogListResponseModel()
            {
                IsSuccess = true,
                Message = "Success",
                Data = lst
            };
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            BlogResponseModel model = new BlogResponseModel();

            AppDbContext db = new AppDbContext();
            var item = db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item == null)
            {
                model.IsSuccess = false;
                model.Message = "No data found!";
                return NotFound(model);
            }
            model.IsSuccess = true;
            model.Message = "Success";
            model.Data = item;
        
            return Ok(model);
        }

        [HttpPost]
        public IActionResult CreateBlog([FromBody] BlogDataModel blog)
        {
            AppDbContext db = new AppDbContext();
            db.Blogs.Add(blog);
            int result = db.SaveChanges();
            string message = result > 0 ? "Saving Successful!" : "Saving Failed!";
            BlogResponseModel model = new BlogResponseModel()
            {
                IsSuccess = result > 0,
                Message = message

            };

            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, [FromBody] BlogDataModel blog)
        {
            BlogResponseModel model = new BlogResponseModel();
            AppDbContext db = new AppDbContext();
            BlogDataModel item = db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item == null)
            {
                model.IsSuccess = false;
                model.Message = "No data found.";
                return NotFound(model);
            }
            item.Blog_Title = blog.Blog_Title;
            item.Blog_Content = blog.Blog_Content;
            item.Blog_Author = blog.Blog_Author;

            int result = db.SaveChanges();
            string message = result > 0 ? "Update Successful!" : "Update Failed!";

            model = new BlogResponseModel()
            {
                IsSuccess = result > 0,
                Message = message

            };
            return Ok(model);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, [FromBody] BlogDataModel blog)
        {
            AppDbContext db = new AppDbContext();
            BlogResponseModel model = new BlogResponseModel();
            var item = db.Blogs.FirstOrDefault(x => x.Blog_Id ==id);

            if(item == null)
            {
                model.IsSuccess = false;
                model.Message = "No data found!";
            }

            if (!string.IsNullOrWhiteSpace(blog.Blog_Title))
            {
                item.Blog_Title = blog.Blog_Title;
            }

            if (!string.IsNullOrWhiteSpace(blog.Blog_Author))
            {
                item.Blog_Author = blog.Blog_Author;
            }

            if (!string.IsNullOrWhiteSpace(blog.Blog_Content))
            {
                item.Blog_Content = blog.Blog_Content;
            }

            int result = db.SaveChanges();
            string message = result > 0 ? "Update Successful!" : "Update Failed!";

            model = new BlogResponseModel()
            {
                IsSuccess = result > 0,
                Message = message
            };

            return Ok(model);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            AppDbContext db = new AppDbContext();
            BlogResponseModel model = new BlogResponseModel();
            BlogDataModel item = db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if(item == null)
            {
                model.IsSuccess=false;
                model.Message = "No Data Found!";
            }

            db.Blogs.Remove(item);
            int result = db.SaveChanges();
            string message = result > 0 ? "Delete Successful!" : "Delete Failed!";

            model = new BlogResponseModel()
            {
                IsSuccess = result > 0,
                Message = message
            };

            return Ok(model);
        }
    }
}
