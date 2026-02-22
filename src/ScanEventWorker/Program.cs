using Microsoft.EntityFrameworkCore;
using Serilog;
using ScanEventWorker.Api;
using ScanEventWorker.Data;
using ScanEventWorker.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

try
{
    var builder = Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureServices((context, services) =>
        {
            services.AddHttpClient<ScanApiClient>((sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var baseUrl = config["ScanApi:BaseUrl"];
                client.BaseAddress = new Uri(baseUrl);
            });

            services.AddDbContext<ScanDbContext>(options =>
                options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ScanProcessor>();

            services.AddHostedService<Worker>();
        });

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}