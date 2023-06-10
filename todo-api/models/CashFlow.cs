using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Globalization;

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


        [NotMapped]
        public DateTime DateTime
        {
            get
            {
                return DateTime.ParseExact(Date.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
        }

        [NotMapped]
        public DateTime Month
        {
            get
            {
                return new DateTime(DateTime.Year, DateTime.Month, 1);
            }
        }

        [NotMapped]
        public DateTime Year
        {
            get
            {
                return new DateTime(DateTime.Year, 1, 1);
            }
        }
    }
}
