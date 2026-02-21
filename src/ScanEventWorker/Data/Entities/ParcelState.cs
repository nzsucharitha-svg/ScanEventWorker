namespace ScanEventWorker.Data.Entities;
public class ParcelState
{
    public long ParcelId { get; set; }
    public long LastEventId { get; set; }
    public string Type { get; set; } = "";
    public string StatusCode { get; set; } = "";
    public string RunId { get; set; } = "";
    public DateTime CreatedDateTimeUtc { get; set; }
    public DateTime? PickupTimeUtc { get; set; }
    public DateTime? DeliveryTimeUtc { get; set; }
}