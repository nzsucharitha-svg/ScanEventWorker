namespace ScanEventWorker.Dtos;
    public class ScanEventDto
    {
        public long EventId { get; set; }
        public long ParcelId { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDateTimeUtc { get; set; }
        public string StatusCode { get; set; }
        public UserDto User { get; set; }
    }
