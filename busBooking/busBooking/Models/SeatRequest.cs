namespace busBooking.Models
{
    public class SeatRequest
    {
        public string SeatNumber { get; set; } = string.Empty;

        public string SeatType { get; set; } = string.Empty;

        public decimal SeatPrice { get; set; }
    }
}
