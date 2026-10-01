namespace busBooking.Models
{
    public class BookingRequest
    {
        public int CustomerId { get; set; }

        public int ScheduleId { get; set; }

        public List<PassengerRequest> Passengers { get; set; }
            = new List<PassengerRequest>();

        public List<SeatRequest> Seats { get; set; }
            = new List<SeatRequest>();
    }
}
