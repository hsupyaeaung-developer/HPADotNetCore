using HPADotNetCore.ConsoleApp.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HPADotNetCore.ConsoleApp.RestClientExamples
{
    public class RestClientExample
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
           // await Update(1016,"update httpclient title", "update httpclient author", "update httpclient content");

            //await Edit(1016);
            await Delete(1016);
            await Read();

        }
        public async Task Read()
        {
            RestClient client = new RestClient();
            RestRequest request= new RestRequest("https://localhost:7250/api/blog", Method.Get);
            //RestRequest request= new RestRequest("https://localhost:7250/api/blog");
            //await client.GetAsync(request);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content;

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

            RestClient client = new RestClient();
            RestRequest request = new RestRequest("https://localhost:7250/api/blog", Method.Post);
            request.AddBody(blog);
            //RestRequest request= new RestRequest("https://localhost:7250/api/blog");
            //await client.GetAsync(request);
            RestResponse response = await client.ExecuteAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content;

                BlogListResponseModel model = JsonConvert.DeserializeObject<BlogListResponseModel>(jsonString);
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
        }

        public async Task Edit(int id)
        {
            RestClient client = new RestClient();
            RestRequest request = new RestRequest($"https://localhost:7250/api/blog/{id}", Method.Get);
            //RestRequest request= new RestRequest("https://localhost:7250/api/blog");
            //await client.GetAsync(request);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content;

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

            RestClient client = new RestClient();
            RestRequest request = new RestRequest($"https://localhost:7250/api/blog/{id}", Method.Patch);
            request.AddBody(blog);
            //RestRequest request= new RestRequest("https://localhost:7250/api/blog");
            //await client.GetAsync(request);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content;

                BlogResponseModel model = JsonConvert.DeserializeObject<BlogResponseModel>(jsonString);
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
        }

        private async Task Delete(int id)
        {
            RestClient client = new RestClient();
            RestRequest request = new RestRequest($"https://localhost:7250/api/blog/{id}", Method.Delete);
           
            //RestRequest request= new RestRequest("https://localhost:7250/api/blog");
            //await client.GetAsync(request);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string jsonString = response.Content;

                BlogResponseModel model = JsonConvert.DeserializeObject<BlogResponseModel>(jsonString);
                Console.WriteLine(JsonConvert.SerializeObject(model, Formatting.Indented));
            }
        }
    }
}
