namespace busBooking.Models
{
    public class PassengerRequest
    {
        public string PassengerName { get; set; } = string.Empty;

        public int Age { get; set; }

        public string Gender { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Email { get; set; }
    }
}
