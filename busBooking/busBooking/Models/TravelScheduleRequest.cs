namespace busBooking.Models
{
    public class TravelScheduleRequest
    {
        public int PartnerId { get; set; }
        public string BusNumber { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int BusTypeId { get; set; }
        public string? BusImage { get; set; }
        public int FromCityId { get; set; }
        public int ToCityId { get; set; }
        public DateTime TravelDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public int TotalSeats { get; set; }
        public int PickupLocationId { get; set; }
        public int DropLocationId { get; set; }
        public decimal Fare { get; set; }
    }
}
