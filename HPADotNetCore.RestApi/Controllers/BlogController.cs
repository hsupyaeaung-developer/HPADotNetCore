using HPADotNetCore.RestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HPADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BlogController(AppDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public IActionResult GetBlogs()
        {
            
            var lst = _db.Blogs.ToList();
            BlogListResponseModel model = new BlogListResponseModel()
            {
                IsSuccess = true,
                Message = "Success",
                Data = lst
            };
            return Ok(model);
        }

        [HttpGet("{pageNo}/{pageSize}")]
        public IActionResult GetBlogs(int pageNo, int pageSize)
        {

            var lst = _db
                .Blogs
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToList();
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

           
            var item = _db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
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
            
            _db.Blogs.Add(blog);
            int result = _db.SaveChanges();
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
            
            BlogDataModel item = _db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item == null)
            {
                model.IsSuccess = false;
                model.Message = "No data found.";
                return NotFound(model);
            }
            item.Blog_Title = blog.Blog_Title;
            item.Blog_Content = blog.Blog_Content;
            item.Blog_Author = blog.Blog_Author;

            int result = _db.SaveChanges();
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
            
            BlogResponseModel model = new BlogResponseModel();
            var item = _db.Blogs.FirstOrDefault(x => x.Blog_Id ==id);

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

            int result = _db.SaveChanges();
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
            
            BlogResponseModel model = new BlogResponseModel();
            BlogDataModel item = _db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if(item == null)
            {
                model.IsSuccess=false;
                model.Message = "No Data Found!";
            }

            _db.Blogs.Remove(item);
            int result = _db.SaveChanges();
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
