using busBooking.Models;
using BusBookingApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace busBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BusBookingDbContext _context;

        public BookingController(BusBookingDbContext context)
        {
            _context = context;
        }

        [HttpGet("seats/{scheduleId}")]
        public async Task<ActionResult> GetBookedSeats(int scheduleId)
        {
            var schedule = await _context.TravelScheduleTbl.FirstOrDefaultAsync(x =>
            x.ScheduleId == scheduleId &&
            x.IsActive);

            if (schedule == null)
            {
                return NotFound("Schedule not found");
            }

            var bookedSeats = await _context.BookingSeatTbl.Where(x =>
            x.ScheduleId == scheduleId &&
            x.Booking!.BoolingStatus == "Confirmed").Select(x => x.SeatNumber).ToListAsync();
            return Ok(bookedSeats);
        }


        [HttpGet("available-seats/{scheduleId}")]
        public async Task<ActionResult> GetAvailableSeats(int scheduleId)
        {
            var schedule = await _context.TravelScheduleTbl.FirstOrDefaultAsync(x =>
            x.ScheduleId == scheduleId &&
            x.IsActive);
            if (schedule == null)
            {
                return NotFound("Schedule not found");
            }

            var bookedSeats = await _context.BookingSeatTbl.Where(x =>
            x.ScheduleId == scheduleId &&
            x.Booking!.BoolingStatus == "Confirmed")
                .Select(x => x.SeatNumber).ToListAsync();

            var availableSeats = new List<string>();
            for (int i = 1; i <= schedule.TotalSeats; i++)
            {
                string seatNumber = $"S{i}";
                if (!bookedSeats.Contains(seatNumber))
                {
                    availableSeats.Add(seatNumber);
                }

            }
            return Ok(new {
                scheduleId = schedule.ScheduleId,
                totalSeats = schedule.TotalSeats,
                bookedSeats,
                availableSeats
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingRequest request)
        {
            
            
           

            var customer = await _context.UserTbl
                .FirstOrDefaultAsync(x =>
                    x.UserId == request.CustomerId &&
                    x.Role == "Customer" &&
                    x.IsActive);

            if (customer == null)
            {
                return BadRequest("Customer not found.");
            }


           
            
          

            var schedule = await _context.TravelScheduleTbl
                .FirstOrDefaultAsync(x =>
                    x.ScheduleId == request.ScheduleId &&
                    x.IsActive);

            if (schedule == null)
            {
                return BadRequest("Schedule not found.");
            }


            
          
            

            if (request.Passengers == null ||
                request.Passengers.Count == 0)
            {
                return BadRequest("At least one passenger is required.");
            }

            if (request.Passengers.Count != request.Seats.Count)
            {
                return BadRequest(
                    "Number of passengers must match number of seats.");
            }


          
            

            var requestedSeats = request.Seats
                .Select(x => x.SeatNumber)
                .ToList();

            if (requestedSeats.Count != requestedSeats.Distinct().Count())
            {
                return BadRequest("Duplicate seats selected.");
            }


            
           
         

            var alreadyBookedSeats = await _context.BookingSeatTbl
                .Where(x =>
                    x.ScheduleId == request.ScheduleId &&
                    x.Booking!.BoolingStatus == "Confirmed" &&
                    requestedSeats.Contains(x.SeatNumber))
                .Select(x => x.SeatNumber)
                .ToListAsync();

            if (alreadyBookedSeats.Any())
            {
                return Conflict(new
                {
                    message = "One or more selected seats are already booked.",
                    seats = alreadyBookedSeats
                });
            }


           
           
      

            if (requestedSeats.Count > schedule.TotalSeats)
            {
                return BadRequest("Invalid number of seats.");
            }


          
            
     

            decimal totalAmount = request.Seats.Sum(x => x.SeatPrice);


           

            var booking = new BookingTbl
            {
                CustomerId = request.CustomerId,
                ScheduleId = request.ScheduleId,
                BookingDate = DateTime.Now,
                TotalSeats = request.Seats.Count,
                TotalAmount = totalAmount,
                BoolingStatus = "Confirmed",
                BookingReference = GenerateBookingReference()
            };

            _context.BookingTbl.Add(booking);

            await _context.SaveChangesAsync();


            
          

            for (int i = 0; i < request.Passengers.Count; i++)
            {
                var passengerRequest = request.Passengers[i];

                var passenger = new BookingPassengerTbl
                {
                    BookingId = booking.BookingId,
                    PassengerName = passengerRequest.PassengerName,
                    Age = passengerRequest.Age,
                    Gender = passengerRequest.Gender,
                    Phone = passengerRequest.Phone,
                    Email = passengerRequest.Email
                };

                _context.BookingPassengerTbl.Add(passenger);

                await _context.SaveChangesAsync();


              
  

                var seatRequest = request.Seats[i];

                var seat = new BookingSeatTbl
                {
                    BookingId = booking.BookingId,
                    ScheduleId = request.ScheduleId,
                    PassengerId = passenger.PassengerId,
                    SeatNumber = seatRequest.SeatNumber,
                    SeatType = seatRequest.SeatType,
                    SeatPrice = seatRequest.SeatPrice
                };

                _context.BookingSeatTbl.Add(seat);
            }

            await _context.SaveChangesAsync();


            return Ok(new
            {
                message = "Booking confirmed successfully.",
                bookingId = booking.BookingId,
                bookingReference = booking.BookingReference,
                totalSeats = booking.TotalSeats,
                totalAmount = booking.TotalAmount
            });
        }
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings(int customerId)
        {
            var bookings = await _context.BookingTbl
                .Where(x => x.CustomerId == customerId)
                .Include(x => x.Schedule)
                .Include(x => x.Passengers)
                .Include(x => x.Seats)
                .OrderByDescending(x => x.BookingDate)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _context.BookingTbl
                .Include(x => x.Schedule)
                .Include(x => x.Customer)
                .Include(x => x.Passengers)
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(x => x.BookingId == id);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(booking);
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var booking = await _context.BookingTbl
                .FirstOrDefaultAsync(x => x.BookingId == id);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            if (booking.BoolingStatus == "Cancelled")
            {
                return BadRequest("Booking is already cancelled.");
            }

            booking.BoolingStatus = "Cancelled";

            await _context.SaveChangesAsync();

            return Ok("Booking cancelled successfully.");
        }

        [HttpGet("partner-bookings")]
        public async Task<IActionResult> GetPartnerBookings(int partnerId)
        {
            var bookings = await _context.BookingTbl
                .Where(x => x.Schedule!.PartnerId == partnerId)
                .Include(x => x.Customer)
                .Include(x => x.Schedule)
                .Include(x => x.Passengers)
                .Include(x => x.Seats)
                .OrderByDescending(x => x.BookingDate)
                .ToListAsync();

            return Ok(bookings);
        }
        [HttpGet("partner-bookings/{id}")]
        public async Task<IActionResult> GetPartnerBooking(int id)
        {
            var booking = await _context.BookingTbl
                .Include(x => x.Customer)
                .Include(x => x.Schedule)
                .Include(x => x.Passengers)
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(x => x.BookingId == id);

            if (booking == null)
            {
                return NotFound("Booking not found.");
            }

            return Ok(booking);
        }
        private string GenerateBookingReference()
        {
            return "BUS-" +
                   DateTime.Now.ToString("yyyyMMddHHmmss") +
                   "-" +
                   Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
    }

}
