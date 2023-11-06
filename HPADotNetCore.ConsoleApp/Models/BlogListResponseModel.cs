using System.Collections.Generic;

namespace HPADotNetCore.ConsoleApp.Models
{
    public class BlogListResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public List<BlogDataModel> Data { get; set; }

    }
}
