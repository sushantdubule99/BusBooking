namespace busBooking.Models
{
    public class BookingSeatTbl
    { 
        public int BookingSeatId { get; set; } 

        public int BookingId { get; set; } 

        public int ScheduleId { get; set; } 

        public int PassengerId { get; set; } 

        public string SeatNumber { get; set; } = string.Empty;

        public string SeatType{  get; set; } = string.Empty; 

        public decimal SeatPrice { get; set; } 

        public BookingTbl? Booking {  get; set; }

        public BookingPassengerTbl? Passenger {  get; set; } 

        public TravelScheduleTbl? Schedule { get; set; }
    }
}
