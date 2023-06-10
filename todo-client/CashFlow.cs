using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{

    public class CashFlow
    {
        [Key]


        public string Id { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public string? CategoryId { get; set; }
        public string? Date { get; set; }
        public string? Remark { get; set; }
        public string? Image { get; set; }
    }
}
