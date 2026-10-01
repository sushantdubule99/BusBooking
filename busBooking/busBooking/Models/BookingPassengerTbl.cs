namespace busBooking.Models
{
    public class BookingPassengerTbl
    { 

        public int PassengerId { get; set; }

        public int BookingId { get; set; }

        public string PassengerName { get; set; } = string.Empty;
        
        public int Age { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string? Phone { get; set; } 

        public string? Email { get; set; }  

        public BookingTbl? Booking { get; set; }

        public ICollection<BookingSeatTbl> Seats { get; set; } = new List<BookingSeatTbl>();
    }
}
