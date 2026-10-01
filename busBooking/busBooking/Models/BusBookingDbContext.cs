using busBooking.Models;

using Microsoft.EntityFrameworkCore;

namespace BusBookingApplication.Data
{
    public class BusBookingDbContext : DbContext
    {
        public BusBookingDbContext(
            DbContextOptions<BusBookingDbContext> options)
            : base(options)
        {
        }


        // Tables
        public DbSet<UserTbl> UserTbl { get; set; }

        public DbSet<MasterTbl> MasterTbl { get; set; }

        public DbSet<TravelScheduleTbl> TravelScheduleTbl { get; set; }

        public DbSet<BookingTbl> BookingTbl { get; set; }

        public DbSet<BookingPassengerTbl> BookingPassengerTbl { get; set; }

        public DbSet<BookingSeatTbl> BookingSeatTbl { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTbl>()
     .HasKey(x => x.UserId);

            modelBuilder.Entity<MasterTbl>()
                .HasKey(x => x.MasterId);

            modelBuilder.Entity<TravelScheduleTbl>()
                .HasKey(x => x.ScheduleId);

            modelBuilder.Entity<BookingTbl>()
                .HasKey(x => x.BookingId);

            modelBuilder.Entity<BookingPassengerTbl>()
                .HasKey(x => x.PassengerId);

            modelBuilder.Entity<BookingSeatTbl>()
                .HasKey(x => x.BookingSeatId);

            modelBuilder.Entity<TravelScheduleTbl>()
    .Property(x => x.Fare)
    .HasPrecision(10, 2);

            modelBuilder.Entity<BookingTbl>()
                .Property(x => x.TotalAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<BookingSeatTbl>()
                .Property(x => x.SeatPrice)
                .HasPrecision(10, 2);


            // =====================================================
            // 1. UserTbl → TravelScheduleTbl
            // =====================================================

            modelBuilder.Entity<TravelScheduleTbl>()
                .HasOne(x => x.Partner)
                .WithMany(x => x.travelSchedules)
                .HasForeignKey(x => x.PartnerId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 2. UserTbl → BookingTbl
            // =====================================================

            modelBuilder.Entity<BookingTbl>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.bookings)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 3. TravelScheduleTbl → BookingTbl
            // =====================================================

            modelBuilder.Entity<BookingTbl>()
                .HasOne(x => x.Schedule)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 4. BookingTbl → BookingPassengerTbl
            // =====================================================

            modelBuilder.Entity<BookingPassengerTbl>()
                .HasOne(x => x.Booking)
                .WithMany(x => x.Passengers)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // 5. BookingTbl → BookingSeatTbl
            // =====================================================

            modelBuilder.Entity<BookingSeatTbl>()
                .HasOne(x => x.Booking)
                .WithMany(x => x.Seats)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            // =====================================================
            // 6. BookingPassengerTbl → BookingSeatTbl
            // =====================================================

            modelBuilder.Entity<BookingSeatTbl>()
                .HasOne(x => x.Passenger)
                .WithMany(x => x.Seats)
                .HasForeignKey(x => x.PassengerId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 7. TravelScheduleTbl → BookingSeatTbl
            // =====================================================

            modelBuilder.Entity<BookingSeatTbl>()
                .HasOne(x => x.Schedule)
                .WithMany()
                .HasForeignKey(x => x.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 8. TravelScheduleTbl → MasterTbl : Company
            // =====================================================

            modelBuilder.Entity<TravelScheduleTbl>()
                .HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 9. TravelScheduleTbl → MasterTbl : BusType
            // =====================================================

            modelBuilder.Entity<TravelScheduleTbl>()
                .HasOne(x => x.BusType)
                .WithMany()
                .HasForeignKey(x => x.BusTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 10. TravelScheduleTbl → MasterTbl : FromCity
            // =====================================================

            modelBuilder.Entity<TravelScheduleTbl>()
                .HasOne(x => x.FromCity)
                .WithMany()
                .HasForeignKey(x => x.FromCityId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 11. TravelScheduleTbl → MasterTbl : ToCity
            // =====================================================

            modelBuilder.Entity<TravelScheduleTbl>()
                .HasOne(x => x.ToCity)
                .WithMany()
                .HasForeignKey(x => x.ToCityId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 12. TravelScheduleTbl → MasterTbl : Pickup
            // =====================================================

            modelBuilder.Entity<TravelScheduleTbl>()
                .HasOne(x => x.PickupLocation)
                .WithMany()
                .HasForeignKey(x => x.PickupLocationId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // 13. TravelScheduleTbl → MasterTbl : Drop
            // =====================================================

            modelBuilder.Entity<TravelScheduleTbl>()
                .HasOne(x => x.DropLocation)
                .WithMany()
                .HasForeignKey(x => x.DropLocationId)
                .OnDelete(DeleteBehavior.Restrict);


            // =====================================================
            // UNIQUE INDEXES
            // =====================================================

            // User email must be unique
            modelBuilder.Entity<UserTbl>()
                .HasIndex(x => x.Email)
                .IsUnique();


            // Same bus cannot be scheduled twice on same date
            modelBuilder.Entity<TravelScheduleTbl>()
                .HasIndex(x => new
                {
                    x.BusNumber,
                    x.TravelDate
                })
                .IsUnique();


            // Booking reference must be unique
            modelBuilder.Entity<BookingTbl>()
                .HasIndex(x => x.BookingReference)
                .IsUnique();


            // Same seat cannot be booked twice for same schedule
            modelBuilder.Entity<BookingSeatTbl>()
                .HasIndex(x => new
                {
                    x.ScheduleId,
                    x.SeatNumber
                })
                .IsUnique();
        }
    }
}
