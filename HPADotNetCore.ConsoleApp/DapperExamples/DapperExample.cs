using Dapper;
using HPADotNetCore.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace HPADotNetCore.ConsoleApp.DapperExamples
{
    public class DapperExample
    {
        private readonly SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = ".",
            InitialCatalog = "AHMTZDotNetCore",
            UserID = "sa",
            Password = "sa@123",
        };

            
        public void Run()
        {

            Create("test title", "test author", "test content");
            Edit(16);
            Update(16, "test16", "author16", "content16");
            Read();
        }
        public void Read()
        {
            string query = "select * from tbl_blog";
            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ToString());
            List<BlogDataModel> lst = db.Query<BlogDataModel>(query).ToList();
            foreach (BlogDataModel item in lst)
            {
                Console.WriteLine(item.Blog_Id);
                Console.WriteLine(item.Blog_Title);
                Console.WriteLine(item.Blog_Author);
                Console.WriteLine(item.Blog_Content);

            }
        }

        public void Create(string title, string author, string content)
        {
            string query = @"INSERT INTO [dbo].[Tbl_Blog]
           ([Blog_Title]
           ,[Blog_Author]
           ,[Blog_Content])
     VALUES
           (@Blog_Title
           ,@Blog_Author
           ,@Blog_Content)";
            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ToString());
            int result = db.Execute(query, new BlogDataModel
            {
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            });
            string message = result > 0 ? "Saving Successful!" : "Saving Failed!";
            Console.WriteLine(message);
        }

        public void Edit(int id)
        {
            string query = "select * from tbl_blog where Blog_Id=@Blog_Id";
            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ToString());
            var item = db.Query<BlogDataModel>(query, new BlogDataModel
            {
                Blog_Id = id
            }).FirstOrDefault();
            Console.WriteLine(item.Blog_Id);
            Console.WriteLine(item.Blog_Title);
            Console.WriteLine(item.Blog_Author);
            Console.WriteLine(item.Blog_Content);
        }
        public void Update(int id, string title, string author, string content)
        {
            string query = @"UPDATE [dbo].[Tbl_Blog]
                            SET [Blog_Title] = @Blog_Title
                            ,[Blog_Author] = @Blog_Author
                            ,[Blog_Content] = @Blog_Content
                            WHERE Blog_Id = @Blog_Id";
            using IDbConnection db = new SqlConnection(sqlConnectionStringBuilder.ToString());
            int result = db.Execute(query, new BlogDataModel
            {
                Blog_Id = id,
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            });
            string message = result > 0 ? "Update Successful!" : "Update Failed!";
            Console.WriteLine(message);
        }
    }
}
