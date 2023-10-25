using HPADotNetCore.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HPADotNetCore.ConsoleApp.EFCoreExamples
{
    public class EFCoreExample
    {
        public void Run()
        {
            Read();
           // Create("EF Core title", "EF Core author", "EF Core content");
            //Edit(21);
           // Update(21);
        }
        private void Read()
        {
            AppDbContext db = new AppDbContext();
            var lst = db.Blogs.ToList();
            foreach (var item in lst)
            {
                Console.WriteLine(item.Blog_Id);
                Console.WriteLine(item.Blog_Title);
                Console.WriteLine(item.Blog_Author);
                Console.WriteLine(item.Blog_Content);

            }
        }
        private void Edit(int id)
        {
            AppDbContext db = new AppDbContext();
            var item = db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item == null)
            {
                Console.WriteLine("There is no data here!");
            }
            Console.WriteLine(item.Blog_Id);
            Console.WriteLine(item.Blog_Title);
            Console.WriteLine(item.Blog_Author);
            Console.WriteLine(item.Blog_Content);
        }
        private void Create(string title,string author,string content)
        {
            AppDbContext db = new AppDbContext();
            BlogDataModel model = new BlogDataModel()
            {
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            };
            db.Blogs.Add(model);
            int result = db.SaveChanges();
            var message = result > 0 ? "Saving Successful!" : "Saving Failed.";
            Console.WriteLine(message);
        }

        private void Update(int id)
        {
            AppDbContext db = new AppDbContext();
            var item = db.Blogs.FirstOrDefault(x => x.Blog_Id == id);
            if (item == null)
            {
                Console.WriteLine("There is no data here.");
            }
            item.Blog_Title = "new title1";
            item.Blog_Author = "new author1";
            item.Blog_Content = "new content1";
            int result = db.SaveChanges();
            var message = result > 0 ? "Update Successful." : "Update Failed.";
            Console.WriteLine(message);
        }
    }
}
