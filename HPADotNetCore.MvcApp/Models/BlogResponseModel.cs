using HPADotNetCore.MvcApp.Models;

namespace HPADotNetCore.MvcApp.Models
{
    public class BlogResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public BlogDataModel Data { get; set; }
    }
}
