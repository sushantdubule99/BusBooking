using BusBookingApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace busBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class SearchController : ControllerBase
    {
        private readonly BusBookingDbContext _context;

        public SearchController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> SearchBuses(
    int fromCityId,
    int toCityId,
    DateTime travelDate,
    int? pickupLocationId = null,
    int? dropLocationId = null)
        {
            var nextDay = travelDate.Date.AddDays(1);

            var query = _context.TravelScheduleTbl
                .Where(x =>
                    x.IsActive &&
                    x.FromCityId == fromCityId &&
                    x.ToCityId == toCityId &&
                    x.TravelDate >= travelDate.Date &&
                    x.TravelDate < nextDay
                );

            if (pickupLocationId.HasValue)
            {
                query = query.Where(x =>
                    x.PickupLocationId == pickupLocationId.Value);
            }

            if (dropLocationId.HasValue)
            {
                query = query.Where(x =>
                    x.DropLocationId == dropLocationId.Value);
            }
            var schedules = await query
    .Include(x => x.Company)
    .Include(x => x.BusType)
    .Include(x => x.FromCity)
    .Include(x => x.ToCity)
    .Include(x => x.PickupLocation)
    .Include(x => x.DropLocation)
    .Select(x => new
    {
        x.ScheduleId,
        x.BusNumber,
        Company = x.Company!.MasterValue,
        BusType = x.BusType!.MasterValue,
        FromCity = x.FromCity!.MasterValue,
        ToCity = x.ToCity!.MasterValue,
        PickupLocation = x.PickupLocation!.MasterValue,
        DropLocation = x.DropLocation!.MasterValue,
        x.TravelDate,
        x.DepartmentTime,
        x.ArrivalTime,
        x.TotalSeats,
        x.Fare,
        x.BusImage
    })
    .ToListAsync();
            if (!schedules.Any())
            {
                return NotFound("No buses found for the selected journey");
            }

            return Ok(schedules);
        }
    }
}
