using busBooking.Models;
using BusBookingApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace busBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly BusBookingDbContext _context;

        public MasterController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var data = await _context.MasterTbl.Where(x => x.IsActive).ToListAsync();

            return Ok(data);
        }

        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            var data = await _context.MasterTbl.Where(x => x.MasterType == "City" && x.IsActive).ToListAsync();
            return Ok(data);
        }

        [HttpGet("bus-types")]
        public async Task<IActionResult> GetBusTypes()
        {
            var data = await _context.MasterTbl.Where(x => x.MasterType == "BusType" && x.IsActive).ToListAsync();
            return Ok(data);
        }

        [HttpGet("pickup-locations")]
        public async Task<IActionResult> GetPickupLocations()
        {
            var data = await _context.MasterTbl.Where(x => x.MasterType == "Pickup" && x.IsActive).ToListAsync();
            return Ok(data);
        }

        [HttpGet("drop-locations")]
        public async Task<IActionResult> GetDropLocations()
        {
            var data = await _context.MasterTbl.Where(x => x.MasterType == "Drop" && x.IsActive).ToListAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MasterTbl master)
        {
            _context.MasterTbl.Add(master);
            await _context.SaveChangesAsync();
            return Ok(master);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MasterTbl master)
        {
            if (id != master.MasterId)
            {
                return BadRequest("MasterId mismatch");
            }
            var existing = await _context.MasterTbl.FindAsync(id);
            if (existing == null)
            {
                return NotFound("Master record not found");
            }

            existing.MasterType = master.MasterType;
            existing.MasterValue = master.MasterValue;
            existing.IsActive = master.IsActive;

            await _context.SaveChangesAsync();

            return Ok(existing);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _context.MasterTbl.FindAsync(id);

            if (existing == null)
            {
                return NotFound("Master record not found");
            }
            existing.IsActive = false;
            await _context.SaveChangesAsync();
            return Ok("Master record deactivated");


        }
    }
}
