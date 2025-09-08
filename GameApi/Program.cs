using GameApi.Models;
using GameApi.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Game API",
        Version = "v1",
        Description = "API para scraping de juegos",
    });
});

builder.Services.AddRouting(routing => routing.LowercaseUrls = true);

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));

});


builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage("Server=MATIAS\\SQLEXPRESS;Database=ScrappingGames;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddHangfireServer(); // Inicia el procesador de trabajos


builder.Services.AddScoped<GameServices>();// Tu servicio que hace la lógica de sincronización
builder.Services.AddTransient<ScrapperGameService>();// Clase que obtiene los juegos
//builder.Services.AddHostedService<GameSyncService>(); // Servicio que corre cada 24h
builder.Services.AddScoped<GameServiceHangFire>();




var app = builder.Build();


// Configurás el job de Hangfire una sola vez al iniciar
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<GameServiceHangFire>();
    service.ConfigurarSincronizacionJuegos();
}

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
