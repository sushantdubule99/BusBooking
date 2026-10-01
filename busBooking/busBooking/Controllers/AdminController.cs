using BusBookingApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace busBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly BusBookingDbContext _context;

        public AdminController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet("pending-partners")]
        public async Task<IActionResult> GetPendingPartners()
        {
            var partners = await _context.UserTbl.Where(x => x.Role == "TravelPartner" && x.ApprovalStatus == "Pending").Select(x => new
            {
                x.UserId,
                x.Name,
                x.Email,
                x.Phone,
                x.Role,
                x.ApprovalStatus,
                x.CreatedDate
            }).ToListAsync();
            return Ok(partners);
        } 

        [HttpPut("approve-partner/{userId}")]
        public async Task<IActionResult> ApprovePartner(int userId)
        {
            var partner = await _context.UserTbl.FirstOrDefaultAsync(x => x.UserId == userId && x.Role == "TravelPartner");
            if(partner == null)
            {
                return NotFound("Travel Partner not found");
            }
            partner.ApprovalStatus = "Approved";
            await _context.SaveChangesAsync();
            return Ok("Travel Partner Approved successfully");
        }

        [HttpPut("reject-partner/{userId}")]
        public async Task<IActionResult> RejectPartner(int userId)
        {
            var partner = await _context.UserTbl.FirstOrDefaultAsync(x => x.UserId == userId && x.Role == "TravelPartner");
            if(partner == null)
            {
                return NotFound("Travel Partner not found");
            }
            partner.ApprovalStatus = "Rejected";
            await _context.SaveChangesAsync();
            return Ok("Travel Partner Rejected");

        }

        [HttpGet("partners")]
        public async Task<IActionResult> GetAllPartners()
        {
            var partners = await _context.UserTbl.Where(x => x.Role == "TravelPartner").Select(x => new
            {
                x.UserId,
                x.Name,
                x.Email,
                x.Phone,
                x.Role,
                x.ApprovalStatus,
                x.IsActive,
                x.CreatedDate
            }).ToListAsync();
            return Ok(partners);
        } 
     
    }
}
