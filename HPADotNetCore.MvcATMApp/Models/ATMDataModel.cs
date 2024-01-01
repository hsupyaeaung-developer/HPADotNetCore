using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HPADotNetCore.MvcATMApp.Models
{
    [Table("Tbl_ATM")]
    public class ATMDataModel
    {
        [Key]
        [Column("Id")]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public int PinNumber { get; set; }
        public string CardNumber { get; set; }

    }
}
