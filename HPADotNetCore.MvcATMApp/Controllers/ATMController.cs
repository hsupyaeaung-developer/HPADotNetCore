using HPADotNetCore.MvcATMApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HPADotNetCore.MvcATMApp.Controllers
{
    public class ATMController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public ATMController(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
           
            return View(model);
        }

        public IActionResult Logout()
        {
            _contextAccessor.HttpContext.Session.Remove("CardNumber");

            return Redirect("/ATM/Logout");

        }
        public IActionResult Deposit()
        {

            return View();
        }

        public IActionResult Withdraw()
        {

            return View();
        }

        public async Task<IActionResult> Balance()
        {
            var CardNumber = _contextAccessor.HttpContext.Session.GetString("CardNumber").ToString();
            var item = await _context.ATMDatas.FirstOrDefaultAsync(x => x.CardNumber == CardNumber);

            BalanceViewModel model = new BalanceViewModel();
            model.CurrentAmount = item.Balance;
            return View(model);
        }

        [HttpPost]
        [ActionName("WithdrawValidate")]
        public async Task<IActionResult> WithdrawValidate(WithdrawViewModel reqModel)
        {
            string removeComma = reqModel.WithdrawAmount.ToString().Replace(",","");
            reqModel.WithdrawAmount = decimal.Parse(removeComma);
            var CardNumber = _contextAccessor.HttpContext.Session.GetString("CardNumber").ToString();
            var item = await _context.ATMDatas.FirstOrDefaultAsync(x => x.CardNumber == CardNumber);

            WithdrawViewModel model = new WithdrawViewModel();

            if (reqModel.WithdrawAmount > item.Balance)
            {
                TempData["Message"] = "Insufficient Balance!";
                TempData["IsSuccess"] = false;
            }
            
            else
            {
                
                model.CardNumber = item.CardNumber;
                model.CurrentBalance = item.Balance;
                model.WithdrawAmount = reqModel.WithdrawAmount;
            }
            return View("WithdrawDetail",model);
        }

        [HttpPost]
        [ActionName("WithdrawConfirm")]
        public async Task<IActionResult> WithdrawConfirm(WithdrawViewModel reqModel)
        {
            WithdrawViewModel model = new WithdrawViewModel();
            var CardNumber = _contextAccessor.HttpContext.Session.GetString("CardNumber").ToString();
            var item = await _context.ATMDatas.FirstOrDefaultAsync(x => x.CardNumber == CardNumber);

            if (reqModel.WithdrawAmount > item.Balance)
            {
                TempData["Message"] = "Insufficient Balance!";
                TempData["IsSuccess"] = false;
            }
            else
            {
                item.Balance = item.Balance - reqModel.WithdrawAmount;
                var result = await _context.SaveChangesAsync();

                string message = result > 0 ? "Withdrawing Successful." : "Withdrawing Failed.";
                TempData["Message"] = message;
                TempData["IsSuccess"] = result > 0;

                model.CardNumber = item.CardNumber;
                model.CurrentBalance = item.Balance;
                model.WithdrawAmount = reqModel.WithdrawAmount;
                
            }
            return View("WithdrawSuccess", model);
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        
        [HttpPost]
        [ActionName("Deposit")]
        public async Task<IActionResult> DepositConfirm(DepositViewModel reqModel)
        {
            var CardNumber = _contextAccessor.HttpContext.Session.GetString("CardNumber").ToString();
            var item = await _context.ATMDatas.FirstOrDefaultAsync(x => x.CardNumber == CardNumber);
            item.Balance = item.Balance + reqModel.DepositAmount;
            var result = await _context.SaveChangesAsync();

            string message = result > 0 ? "Saving Successful." : "Saving Failed.";
           
            MessageModel model = new MessageModel(result > 0, message);
            return Json(model);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginConfirm(LoginViewModel reqModel)
        {
            var atm = await _context.ATMDatas.AsNoTracking().FirstOrDefaultAsync(x => x.CardNumber == reqModel.CardNumber && x.PinNumber == reqModel.PinNumber);
            if(atm == null)
            {
                TempData["Message"] = "Not Found!";
                return Redirect("/ATM/Login");
            }
            else
            {
                _contextAccessor.HttpContext.Session.SetString("CardNumber", atm.CardNumber);
                TempData["Message"] = "Success!";
            }
            return View("Dashboard");
        }
        public IActionResult Register()
        {
            ATMDataModel model = new ATMDataModel();
            GenerateNumberReturnModel generateModel = new GenerateNumberReturnModel();
            bool IsValid = false;

            do
            {
                generateModel = GenerateNumberWithLuhnsAlgorithm();
                IsValid = generateModel.IsValid;
                
            } while (!IsValid);

            model.CardNumber = generateModel.GeneratedNumber;
            return View(model);
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> RegisterInfoSave(ATMDataModel reqModel)
        {
            await _context.ATMDatas.AddAsync(reqModel);
            var result = await _context.SaveChangesAsync();
            string message = result > 0 ? "Saving Successful." : "Saving Failed.";
            
            MessageModel model = new MessageModel(result > 0, message);
            return Json(model);
        
        }

        public GenerateNumberReturnModel GenerateNumberWithLuhnsAlgorithm ()
        {
            GenerateNumberReturnModel model = new GenerateNumberReturnModel();
            string GenerateNumber = "";
            for (int i = 0; i < 16; i++)
            {
                GenerateNumber += GenerateData(1, 10).ToString();
            }
            model.IsValid = checkLuhn(GenerateNumber);
            model.GeneratedNumber = GenerateNumber;
            return model;
        }
        public int GenerateData(int from, int to)
        {
            Random random = new Random();
            return random.Next(from, to);
        }

        // Returns true if given 
        // card number is valid
        static bool checkLuhn(string cardNo)
        {
            int nDigits = cardNo.Length;

            int nSum = 0;
            bool isSecond = false;
            for (int i = nDigits - 1; i >= 0; i--)
            {

                int d = cardNo[i] - '0';

                if (isSecond == true)
                    d = d * 2;

                // We add two digits to handle
                // cases that make two digits 
                // after doubling
                nSum += d / 10;
                nSum += d % 10;

                isSecond = !isSecond;
            }
            return (nSum % 10 == 0);
        }

       
    }
}

