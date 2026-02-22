
using ScanEventWorker.Data;
using ScanEventWorker.Data.Entities;
using ScanEventWorker.Dtos;

namespace ScanEventWorker.Services;
public class ScanProcessor
{
    private readonly ScanDbContext _db;
    private readonly ILogger<ScanProcessor> _logger;

    public ScanProcessor(ScanDbContext db, ILogger<ScanProcessor> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task ProcessAsync(ScanEventDto scan)
    {
        try
        {
            var parcel = await _db.Parcels.FindAsync(scan.ParcelId)
                ?? new ParcelState { ParcelId = scan.ParcelId };

            parcel.LastEventId = scan.EventId;
            parcel.Type = scan.Type;
            parcel.StatusCode = scan.StatusCode;
            parcel.RunId = scan.User?.RunId ?? "";
            parcel.CreatedDateTimeUtc = scan.CreatedDateTimeUtc;

            if (scan.Type == "PICKUP")
                parcel.PickupTimeUtc ??= scan.CreatedDateTimeUtc;

            if (scan.Type == "DELIVERY")
                parcel.DeliveryTimeUtc ??= scan.CreatedDateTimeUtc;

            _db.Parcels.Update(parcel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed processing EventId {EventId} ParcelId {ParcelId}",scan.EventId,scan.ParcelId);
        }
    }
}