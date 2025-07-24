using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameApi.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? Discount { get; set; }
        public DateTime? Fecha { get; set; }



    }
}
