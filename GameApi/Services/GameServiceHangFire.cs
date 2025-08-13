using Hangfire;

namespace GameApi.Services
{
    public class GameServiceHangFire
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GameServiceHangFire> _logger;
        public GameServiceHangFire(IServiceProvider serviceProvider, ILogger<GameServiceHangFire> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public void ConfigurarSincronizacionJuegos()
        {
          /*  RecurringJob.AddOrUpdate(
                "SincronizarJuegos",
                () => SincronizarAsync(),
                Cron.Daily(3) // Cada día a las 3 AM
            );*/

            RecurringJob.AddOrUpdate(
               "SincronizarJuegos",
               () => SincronizarAsync(),
               "14 01 * * *"
           );
        
        }
  
        public async Task SincronizarAsync()
        {

            _logger.LogInformation("Sincronización de juegos iniciada a las {time}", DateTimeOffset.Now);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var gameService = scope.ServiceProvider.GetRequiredService<GameServices>();
                    await gameService.SincronizarJuegosAsync();
                }
                _logger.LogInformation("Sincronización completada a las {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la sincronización de juegos.");
            }

        }



    }
}
