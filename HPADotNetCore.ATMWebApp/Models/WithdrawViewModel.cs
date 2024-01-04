namespace HPADotNetCore.ATMWebApp.Models
{
    public class WithdrawViewModel
    {
        public decimal WithdrawAmount { get; set; }
        public string CardNumber { get; set; }
        public decimal CurrentBalance { get; set; }

    }
}
