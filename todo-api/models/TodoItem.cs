using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class TodoItem {
        [Key] 
        public int Id { get; set; }
        public int Password { get; set; }
        public string? Nickname { get; set; }

    }
}