using Microsoft.EntityFrameworkCore;
using ScanEventWorker.Api;
using ScanEventWorker.Data;
using ScanEventWorker.Data.Entities;
using ScanEventWorker.Services;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ScanApiClient _client;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceScopeFactory scopeFactory, ScanApiClient client, ILogger<Worker> logger)
    {
        _scopeFactory = scopeFactory;
        _client = client;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ScanDbContext>();
            var processor = scope.ServiceProvider.GetRequiredService<ScanProcessor>();

            var state =  await db.WorkerStates.OrderByDescending(x => x.Id).FirstOrDefaultAsync()
                        ?? new WorkerState { Id = 1, LastProcessedEventId = 0 };
            var parcels = await db.ParcelStates
    .OrderByDescending(x => x.ParcelId)
    .FirstOrDefaultAsync();


            var response = await _client.GetEvents(state.LastProcessedEventId + 1, 100, stoppingToken);

            if (response?.ScanEvents?.Any() == true)
            {
                foreach (var scan in response.ScanEvents.OrderBy(x => x.EventId))
                {
                    await processor.ProcessAsync(scan);
                    state.LastProcessedEventId = scan.EventId;
                }

                db.WorkerStates.Update(state);
                await db.SaveChangesAsync(stoppingToken);

                _logger.LogInformation("Checkpoint saved: {id}", state.LastProcessedEventId);
            }

            await Task.Delay(3000, stoppingToken);
        }
    }
}