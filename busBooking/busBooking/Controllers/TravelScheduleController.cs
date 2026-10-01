using busBooking.Models;
using BusBookingApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace busBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelScheduleController : ControllerBase
    {
        private readonly BusBookingDbContext _context;
        public TravelScheduleController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchedule(TravelScheduleRequest request)
        {
            // Check Travel Partner
            var partner = await _context.UserTbl
                .FirstOrDefaultAsync(x =>
                    x.UserId == request.PartnerId &&
                    x.Role == "TravelPartner");

            if (partner == null)
            {
                return BadRequest("Travel Partner not found.");
            }

            // Check Travel Partner approval
            if (partner.ApprovalStatus != "Approved")
            {
                return BadRequest("Travel Partner is not approved.");
            }

            // Check duplicate bus number on same date
            var duplicateBus = await _context.TravelScheduleTbl
                .AnyAsync(x =>
                    x.BusNumber == request.BusNumber &&
                    x.TravelDate.Date == request.TravelDate.Date &&
                    x.IsActive);

            if (duplicateBus)
            {
                return BadRequest(
                    "This bus is already scheduled for this date.");
            }

            // Check From and To city are different
            if (request.FromCityId == request.ToCityId)
            {
                return BadRequest(
                    "From City and To City cannot be the same.");
            }

            // Check seats
            if (request.TotalSeats <= 0)
            {
                return BadRequest(
                    "Total seats must be greater than zero.");
            }

            // Check fare
            if (request.Fare <= 0)
            {
                return BadRequest(
                    "Fare must be greater than zero.");
            }

            // Create Travel Schedule entity
            var schedule = new TravelScheduleTbl
            {
                PartnerId = request.PartnerId,
                BusNumber = request.BusNumber,
                CompanyId = request.CompanyId,
                BusTypeId = request.BusTypeId,
                BusImage = request.BusImage,
                FromCityId = request.FromCityId,
                ToCityId = request.ToCityId,
                TravelDate = request.TravelDate,
                
                ArrivalTime = request.ArrivalTime,
                TotalSeats = request.TotalSeats,
                PickupLocationId = request.PickupLocationId,
                DropLocationId = request.DropLocationId,
                Fare = request.Fare,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            // Save to database
            _context.TravelScheduleTbl.Add(schedule);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Travel schedule created successfully.",
                scheduleId = schedule.ScheduleId
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule(int id)
        {
            var schedule = await _context.TravelScheduleTbl.Include(x => x.Company)
                .Include(x => x.BusType)
                .Include(x => x.FromCity)
                .Include(x => x.ToCity)
                .Include(x => x.PickupLocation)
                .Include(x => x.DropLocation).FirstOrDefaultAsync(x => x.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound("Schedule not found");
            }
            return Ok(schedule);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchedule(
            int id,
            TravelScheduleTbl schedule)
        {
            var existing = await _context.TravelScheduleTbl.FindAsync(id);
            if (existing == null)
            {
                return NotFound("Schedule not found");
            }

            bool duplicate = await _context.TravelScheduleTbl.AnyAsync(x =>
            x.ScheduleId != id &&
            x.BusNumber == schedule.BusNumber &&
            x.TravelDate == schedule.TravelDate.Date);

            if (duplicate)
            {
                return BadRequest("This bus is already scheduled for the selected date");
            }
            existing.BusNumber = schedule.BusNumber;
            existing.CompanyId = schedule.CompanyId;
            existing.BusTypeId = schedule.BusTypeId;
            existing.BusImage = schedule.BusImage;
            existing.FromCityId = schedule.FromCityId;
            existing.ToCityId = schedule.ToCityId;
            existing.TravelDate = schedule.TravelDate;
            existing.DepartmentTime = schedule.DepartmentTime;
            existing.ArrivalTime = schedule.ArrivalTime;
            existing.TotalSeats = schedule.TotalSeats;
            existing.PickupLocationId = schedule.PickupLocationId;
            existing.DropLocationId = schedule.DropLocationId;
            existing.Fare = schedule.Fare;
            await _context.SaveChangesAsync();
            return Ok(existing);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.TravelScheduleTbl.FindAsync(id);
            if(schedule ==null)
            {
                return NotFound("Schedule not found");
            }

            schedule.IsActive = false;
            await _context.SaveChangesAsync();
            return Ok("Schedule deactivated successfully");

        }
    }
}
