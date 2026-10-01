namespace busBooking.Models
{
    public class BookingTbl
    { 

        public int BookingId { get; set; }

        public int CustomerId { get; set; }

        public int ScheduleId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public int TotalSeats { get; set; }

        public decimal TotalAmount { get; set; }

        public string BoolingStatus { get; set; } = "Confirmed";

        public string BookingReference { get; set; } = string.Empty;

        public UserTbl? Customer {  get; set; }  

        public TravelScheduleTbl?Schedule { get; set; }

        public ICollection<BookingPassengerTbl> Passengers { get; set; } = new List<BookingPassengerTbl>(); 

        public ICollection<BookingSeatTbl> Seats { get; set; } = new List<BookingSeatTbl>();


    }
}
