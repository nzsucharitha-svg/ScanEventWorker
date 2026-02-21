namespace ScanEventWorker.Data.Entities;
public class WorkerState
{
    public int Id { get; set; }
    public long LastProcessedEventId { get; set; }
}