using Dapper;
using HPADotNetCore.RestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HPADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDapperController : ControllerBase
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

        public BlogDapperController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DbConnection");
            string connectionString2 = configuration.GetSection("SqlDbConnection").Value;
            string connectionString3 = configuration.GetSection("MyDbConnections:MyDb:DbConnection").Value;
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "select * from tbl_blog";
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ToString());
            List<BlogDataModel> lst = db.Query<BlogDataModel>(query).ToList();
            
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

            string query = "select * from tbl_blog where Blog_Id=@Blog_Id";
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ToString());
            var item = db.Query<BlogDataModel>(query, new BlogDataModel
            {
                Blog_Id = id
            }).FirstOrDefault();
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
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([Blog_Title]
           ,[Blog_Author]
           ,[Blog_Content])
     VALUES
           (@Blog_Title
           ,@Blog_Author
           ,@Blog_Content)";
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ToString());
            int result = db.Execute(query, new BlogDataModel
            {
                Blog_Title = blog.Blog_Title,
                Blog_Author = blog.Blog_Author,
                Blog_Content = blog.Blog_Content
            });
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
            string query = @"UPDATE [dbo].[Tbl_Blog]
                            SET [Blog_Title] = @Blog_Title
                            ,[Blog_Author] = @Blog_Author
                            ,[Blog_Content] = @Blog_Content
                            WHERE Blog_Id = @Blog_Id";
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ToString());
            int result = db.Execute(query, new BlogDataModel
            {
                Blog_Id = id,
                Blog_Title = blog.Blog_Title,
                Blog_Author = blog.Blog_Author,
                Blog_Content = blog.Blog_Content
            });
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
            string query = @"UPDATE [dbo].[Tbl_Blog]
                            SET [Blog_Title] = @Blog_Title
                            ,[Blog_Author] = @Blog_Author
                            ,[Blog_Content] = @Blog_Content
                            WHERE Blog_Id = @Blog_Id";
            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ToString());
            
            BlogDataModel item = new BlogDataModel();
     
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

            int result = db.Execute(query, item);
            string message = result > 0 ? "Update Successful!" : "Update Failed!";

            BlogResponseModel model = new BlogResponseModel()
            {
                IsSuccess = result > 0,
                Message = message
            };

            return Ok(model);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            string query = $@"DELETE FROM [dbo].[Tbl_Blog]
                     WHERE [Blog_Id] = @Blog_Id";

            BlogDataModel blog = new BlogDataModel()
            {
                Blog_Id = id,
            };

            using IDbConnection db = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            var result = db.Execute(query, blog);
            string message = result > 0 ? "Delete Successful." : "Delete Failed.";

            BlogResponseModel model = new BlogResponseModel()
            {
                IsSuccess = result > 0,
                Message = message
            };

            return Ok(model);
        }
    }
}
