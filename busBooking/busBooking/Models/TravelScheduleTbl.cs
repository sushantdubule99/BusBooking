namespace busBooking.Models
{
    public class TravelScheduleTbl 
    { 
        public int ScheduleId { get; set; }

        public int PartnerId { get; set; }

        public string BusNumber { get; set; } = string.Empty;

        public int CompanyId { get; set; }

        public int BusTypeId { get; set; }

        public string? BusImage { get; set; }

        public int FromCityId { get; set; }

        public int ToCityId { get; set; }

        public DateTime TravelDate { get; set; }

        public TimeSpan DepartmentTime { get; set; }

        public TimeSpan ArrivalTime { get; set; } 

        public int TotalSeats { get; set; }

        public int PickupLocationId { get; set; }

        public int DropLocationId { get; set; } 

        public decimal Fare { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now; 

        public UserTbl? Partner { get; set; }

        public MasterTbl? Company { get; set; }

        public MasterTbl? BusType { get; set; } 

        public MasterTbl? FromCity { get; set; }

        public MasterTbl? ToCity { get; set; } 

        public MasterTbl? PickupLocation { get; set; } 

        public MasterTbl? DropLocation { get; set; } 

        public ICollection<BookingTbl> Bookings { get; set; } = new List<BookingTbl>();








    }
}
