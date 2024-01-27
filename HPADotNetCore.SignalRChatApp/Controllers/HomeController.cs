using HPADotNetCore.SignalRChatApp.Hubs;
using HPADotNetCore.SignalRChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace HPADotNetCore.SignalRChatApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHubContext<ChatHub> _chatContext;
        public HomeController(ILogger<HomeController> logger, IHubContext<ChatHub> chatContext)
        {
            _logger = logger;
            _chatContext = chatContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

        private static int count = 0;
        public async Task<IActionResult> Order()
        {
            count++;
            await _chatContext.Clients.All.SendAsync("OrderReceiveMessage", count);
            return Redirect("/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}