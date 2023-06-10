using System.ComponentModel.DataAnnotations.Schema;

namespace todo_api.models
{
    public class StatisticData
    {
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }


    }
}
