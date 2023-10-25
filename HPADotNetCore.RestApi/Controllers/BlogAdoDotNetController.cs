using HPADotNetCore.RestApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HPADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNetController : ControllerBase
    {
        private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;

        public BlogAdoDotNetController (IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DbConnection");
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        }

        [HttpGet]
        public IActionResult GetBlogs()
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "select * from tbl_blog";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            List<BlogDataModel> lst = new List<BlogDataModel>();
            foreach (DataRow dr in dt.Rows)
            {
                BlogDataModel item = new BlogDataModel
                {
                    Blog_Id = Convert.ToInt32(dr["Blog_Id"]),
                    Blog_Title = dr["BlogTitle"].ToString(),
                    Blog_Author = dr["Blog_Author"].ToString(),
                    Blog_Content = dr["Blog_Content"].ToString()

                };
                lst.Add(item);
            }
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
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = "select * from tbl_blog where Blog_Id=@Blog_Id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            connection.Close();

            if (dt.Rows.Count == 0)
            {
                model.IsSuccess = false;
                model.Message = "No data found!";
                return NotFound(model);
            }

            DataRow dr = dt.Rows[0];
            BlogDataModel item = new BlogDataModel
            {
                Blog_Id = Convert.ToInt32(dr["Blog_Id"]),
                Blog_Title = dr["BlogTitle"].ToString(),
                Blog_Author = dr["Blog_Author"].ToString(),
                Blog_Content = dr["Blog_Content"].ToString()

            };

            model.IsSuccess = true;
            model.Message = "Success";
            model.Data = item;

            return Ok(model);
        }

        [HttpPost]
        public IActionResult CreateBlog([FromBody] BlogDataModel blog)
        {
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([Blog_Title]
           ,[Blog_Author]
           ,[Blog_Content])
     VALUES
           (@Blog_Title
           ,@Blog_Author
           ,@Blog_Content)";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            cmd.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            cmd.Parameters.AddWithValue("@Blog_Content", blog.Blog_Content);
            int result = cmd.ExecuteNonQuery();

            connection.Close();
            string message = result > 0 ? "Saving Successful.!" : "Saving Failed.";
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
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            string query = @"UPDATE [dbo].[Tbl_Blog]
   SET [Blog_Title] = @Blog_Title
      ,[Blog_Author] = @Blog_Author
      ,[Blog_Content] = @Blog_Content
 WHERE Blog_Id = @Blog_Id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);
            cmd.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            cmd.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            cmd.Parameters.AddWithValue("@Blog_Content", blog.Blog_Content);
            int result = cmd.ExecuteNonQuery();

            connection.Close();
            string message = result > 0 ? "Updating Successful.!" : "Updating Failed.";

            BlogResponseModel model = new BlogResponseModel()
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
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

           
            string query = @"UPDATE [dbo].[Tbl_Blog]
   SET [Blog_Title] = @Blog_Title
      ,[Blog_Author] = @Blog_Author
      ,[Blog_Content] = @Blog_Content
 WHERE Blog_Id = @Blog_Id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);

            if (!string.IsNullOrWhiteSpace(blog.Blog_Title))
            {
                cmd.Parameters.AddWithValue("@Blog_Title", blog.Blog_Title);
            }

            if (!string.IsNullOrWhiteSpace(blog.Blog_Author))
            {
                cmd.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            }

            if (!string.IsNullOrWhiteSpace(blog.Blog_Content))
            {
                cmd.Parameters.AddWithValue("@Blog_Author", blog.Blog_Author);
            }

            int result = cmd.ExecuteNonQuery();

            connection.Close();
            string message = result > 0 ? "Updating Successful.!" : "Updating Failed.";
           
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
            string query = $@"DELETE FROM[dbo].[Tbl_Blog] WHERE[Blog_ID] = @Blog_ID";
            SqlConnection connection = new SqlConnection(_sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Blog_Id", id);

            int result = cmd.ExecuteNonQuery();
            connection.Close();

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
