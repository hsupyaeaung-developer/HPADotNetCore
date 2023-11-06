using HPADotNetCore.ConsoleApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HPADotNetCore.ConsoleApp.HttpClientExamples
{
    public class HttpClientExample
    {
        private readonly SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = ".",
            InitialCatalog = "AHMTZDotNetCore",
            UserID = "sa",
            Password = "sa@123",
        };
        public async Task Run()
        {

            //await Create("httpclient title", "httpclient author", "httpclient content");
            //await Update(1015,"update httpclient title", "update httpclient author", "update httpclient content");

            //await Edit(1015);
            await Delete(1015);
            await Read();

        }
        public async Task Read()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:7250/api/blog");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                
                BlogListResponseModel model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonString);
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }

        }

        public async Task Create(string title, string author, string content)
        {
            BlogDataModel blog = new BlogDataModel
            {
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            };

            string blogJson = JsonConvert.SerializeObject(blog);
            HttpContent httpContent = new StringContent(blogJson, Encoding.UTF8, Application.Json);
            HttpClient client = new HttpClient();
            
            var response = await client.PostAsync("https://localhost:7250/api/blog", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                BlogListResponseModel model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonString);
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
        }

        public async Task Edit(int id)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:7250/api/blog/{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                BlogResponseModel model = JsonConvert.DeserializeObject<BlogResponseModel>(jsonString);
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
        }
        public async Task Update(int id, string title, string author, string content)
        {
            BlogDataModel blog = new BlogDataModel
            {
                Blog_Title = title,
                Blog_Author = author,
                Blog_Content = content
            };

            string blogJson = JsonConvert.SerializeObject(blog);
            HttpContent httpContent = new StringContent(blogJson, Encoding.UTF8, Application.Json);
            
            HttpClient client = new HttpClient();
            var response = await client.PatchAsync($"https://localhost:7250/api/blog/{id}", httpContent);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                BlogResponseModel model = JsonConvert.DeserializeObject<BlogResponseModel>(jsonString);
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
        }

        private async Task Delete(int id)
        {
            HttpClient client = new HttpClient();
            var response = await client.DeleteAsync($"https://localhost:7250/api/blog/{id}");
            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();

                BlogResponseModel model = JsonConvert.DeserializeObject<BlogResponseModel>(jsonString);
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
        }
    }
}
