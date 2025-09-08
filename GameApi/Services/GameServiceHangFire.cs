using Hangfire;

namespace GameApi.Services
{
    public class GameServiceHangFire
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GameServiceHangFire> _logger;
        private readonly IRecurringJobManager _recurringJobManager;

        public GameServiceHangFire(IServiceProvider serviceProvider, 
                                   ILogger<GameServiceHangFire> logger,
                                   IRecurringJobManager recurringJobManager)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _recurringJobManager = recurringJobManager;
        }

        public void ConfigurarSincronizacionJuegos()
        {

            var options = new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.Local
            };

            _recurringJobManager.AddOrUpdate(
                "SincronizarJuegos",
                () => SincronizarAsync(),
                "49 02 * * *",  
                options
            );
        }


        [AutomaticRetry(Attempts = 3, DelaysInSeconds = new int[] { 60, 120, 300 })]
        public async Task SincronizarAsync()
        {
            Console.WriteLine("Iniciando sincronización de juegos...");

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
                throw;
            }

        }



    }
}
