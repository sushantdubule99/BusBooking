namespace busBooking.Models
{
    public class UserTbl
    { 
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string ApprovalStatus { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now; 

        public ICollection<TravelScheduleTbl> travelSchedules { get; set; } = new List<TravelScheduleTbl>();

        public ICollection<BookingTbl> bookings { get; set; } = new List<BookingTbl>();







    }
}
