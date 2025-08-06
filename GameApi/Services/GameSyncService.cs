

namespace GameApi.Services
{
    public class GameSyncService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GameSyncService> _logger;

        public GameSyncService(IServiceProvider serviceProvider, ILogger<GameSyncService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRun = DateTime.Today.AddHours(3); // 03:00 AM

                // Si ya pasaron las 3 AM de hoy, programar para mañana
                if (now > nextRun)
                    nextRun = nextRun.AddDays(1);

                var delay = nextRun - now;

                // Esperar hasta la hora programada
                await Task.Delay(delay, stoppingToken);


                _logger.LogInformation("Sincronización de juegos iniciada a las {time}", DateTimeOffset.Now);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var gameService = scope.ServiceProvider.GetRequiredService<GameServices>();
                        await gameService.SincronizarJuegosAsync();
                    }

                    _logger.LogInformation("Sincronización completada a las {time}",DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error durante la sincronización de juegos.");
                }

                // Esperar 24 horas
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

    }
}
