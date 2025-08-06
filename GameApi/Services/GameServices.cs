using GameApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GameApi.Services
{
    public class GameServices
    {
        private readonly ScrapperGameService _scraper;
        private readonly MyDbContext _db;

        public GameServices(ScrapperGameService scraper, MyDbContext db)
        {
            _scraper = scraper;
            _db = db;
        }

        public async Task SincronizarJuegosAsync()
        {
            var juegosScrapeados = _scraper.ObtenerJuegos();

            foreach (var juego in juegosScrapeados)
            {
                var juegoExistente = await _db.Games
                    .FirstOrDefaultAsync(j => j.Name == juego.Name);

                if (juegoExistente != null)
                {
                    juegoExistente.Price = juego.Price;
                    juegoExistente.Discount = juego.Discount;
                    juegoExistente.EsPrecompra = juego.EsPrecompra;
                    juegoExistente.Image = juego.Image;
                    juegoExistente.FechaActualizacion = DateTime.UtcNow;
                }
                else
                {
                    juego.FechaActualizacion = DateTime.UtcNow;
                    await _db.Games.AddAsync(juego);
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}
