using GameApi.DTO;
using GameApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Threading.Tasks;
using GameApi.Mappers;

namespace GameApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : Controller
    {

        private readonly MyDbContext _db;
        public GameController(MyDbContext db)
        {
            _db = db;
        }


        [HttpGet("AllGames")]

        public async Task<IActionResult> GetAll()
        {
          /*  var games = await _db.Games.Select(g => new GameDto
            {
                GameId = g.GameId,
                Name = g.Name,
                Image = g.Image,
                Link = g.Link,
                Price = g.Price,
                Discount = g.Discount
            }).ToListAsync();*/


            var games = await _db.Games
                .Select(g => GameMapper.ToDto(g))
                .ToListAsync();

            return new OkObjectResult(games);
        }

        [HttpGet("AllGamesPaginated")]
        public async Task<IActionResult> GetAllPaginated(int pageNumber = 1, int pageSize = 25)
        {
            var gameCount = await _db.Games.CountAsync();

            /*var games = await _db.Games
                .OrderBy(g => g.GameId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GameDto
                {
                    GameId = g.GameId,
                    Name = g.Name,
                    Image = g.Image,
                    Link = g.Link,
                    Price = g.Price,
                    Discount = g.Discount
                }).ToListAsync();*/

            var games = await _db.Games
               .OrderBy(g => g.GameId)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .Select(g => GameMapper.ToDto(g)) // ´mapper
               .ToListAsync();

/*
           var dataGames = new {
               TotalItems = gameCount,
               PageSize = pageSize,
               PageNumber = pageNumber,
               TotalPages = (int)Math.Ceiling((double)gameCount / pageSize),
               Items = games
           };*/

            var dataGames = PageMapper.ToPagedResult(games, gameCount, pageNumber, pageSize);



            return new OkObjectResult(dataGames);
        }

         [HttpGet("{id}")]
          public async Task<IActionResult> GetGame(int id)
          {
            var game = await _db.Games.Where(g => g.GameId == id).FirstOrDefaultAsync();

            if(game == null)
            {
                return NotFound();
            }

            var newGame = new GameDto
            {
                GameId = game.GameId,
                Name = game.Name,
                Image = game.Image,
                Link = game.Link,
                Price = game.Price,
                Discount = game.Discount
            };


              return new OkObjectResult(newGame);
          }

        [HttpGet("Filter/")]

        public async Task<IActionResult> GetJuegosFiltrados(
            [FromQuery] int? rangoMin,
            [FromQuery] int? rangoMax,
            [FromQuery] int? discount,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = _db.Games.AsQueryable();//Convierte un DbSet<T> en un objeto de consulta 

            if (rangoMin > rangoMax)
            {
                return BadRequest("El rango minimo no puede ser mayor al rango maximo");
            }
            if (rangoMin.HasValue)
            {
                query = query.Where(g => g.Price >= rangoMin);
            }
            if (rangoMax.HasValue)
            {
                query = query.Where(g => g.Price <= rangoMax);
            }
            if (discount > 0)
            {
                query = query.Where(g => g.Discount >= discount);
            }

            var totalGames = await query.CountAsync();

         /*   var games = await query
                .OrderBy(g => g.GameId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new GameDto
                {
                    GameId = g.GameId,
                    Name = g.Name,
                    Image = g.Image,
                    Link = g.Link,
                    Price = g.Price,
                    Discount = g.Discount
                })
                .ToListAsync();*/



            var games = await query
                .OrderBy(g => g.GameId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(g => GameMapper.ToDto(g))
                .ToListAsync();


         /*   var result = new
            {
                TotalItems = totalGames,
                PageSize = pageSize,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling((double)totalGames / pageSize),
                Items = games
            };*/

            var result = PageMapper.ToPagedResult(games, totalGames, pageNumber, pageSize);

            return new OkObjectResult(result);
        }

    }
}
