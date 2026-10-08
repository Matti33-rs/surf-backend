using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;
using Surf.Api.Data;
using Surf.Api.Options;
using Surf.Api.Services;

// Bootstrap logger captures startup errors before the host's configuration is loaded.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog((services, configuration) => configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services));

    // Options
    builder.Services.Configure<WhatsAppOptions>(builder.Configuration.GetSection(WhatsAppOptions.SectionName));
    builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection(RedisOptions.SectionName));

    // EF Core (MySQL)
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySQL(builder.Configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' is not configured.")));

    // Redis
    builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    {
        var redisOptions = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Connection string 'Redis' is not configured."));
        redisOptions.AbortOnConnectFail = false;
        return ConnectionMultiplexer.Connect(redisOptions);
    });

    // Services
    builder.Services.AddScoped<WhatsAppWebhookService>();

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
