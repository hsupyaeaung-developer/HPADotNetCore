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
    }
}
