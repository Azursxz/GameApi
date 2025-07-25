using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameApi.DTO
{
    public class GameDto
    {
        public int GameId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        public decimal? Price { get; set; }
        public int? Discount { get; set; }
    }
}
