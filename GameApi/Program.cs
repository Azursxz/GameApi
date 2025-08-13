using GameApi.Models;
using GameApi.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Hangfire.SqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(routing => routing.LowercaseUrls = true);

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));

});


builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage("Server=MATIAS\\SQLEXPRESS;Database=ScrappingGames;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddHangfireServer(); // Inicia el procesador de trabajos


builder.Services.AddScoped<GameServices>();// Tu servicio que hace la lógica de sincronización
builder.Services.AddScoped<ScrapperGameService>();// Clase que obtiene los juegos
//builder.Services.AddHostedService<GameSyncService>(); // Servicio que corre cada 24h
builder.Services.AddScoped<GameServiceHangFire>();




var app = builder.Build();


// Middleware personalizado para configurar trabajos
app.Use(async (context, next) =>
{
    if (!context.Request.Path.StartsWithSegments("/hangfire")) // Evita ejecución múltiple
    {
        using (var scope = app.Services.CreateScope())
        {
            var service = scope.ServiceProvider.GetRequiredService<GameServiceHangFire>();
            service.ConfigurarSincronizacionJuegos();
        }
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
