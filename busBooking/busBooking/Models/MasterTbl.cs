namespace busBooking.Models
{
    public class MasterTbl
    { 
        public int MasterId { get; set; }

        public string MasterType { get; set; } = string.Empty;

        public string MasterValue { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;


    }
}
