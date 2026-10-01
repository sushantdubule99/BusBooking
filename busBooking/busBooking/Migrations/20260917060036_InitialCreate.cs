using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace busBooking.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterTbl",
                columns: table => new
                {
                    MasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MasterType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MasterValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTbl", x => x.MasterId);
                });

            migrationBuilder.CreateTable(
                name: "UserTbl",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTbl", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TravelScheduleTbl",
                columns: table => new
                {
                    ScheduleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    BusNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    BusTypeId = table.Column<int>(type: "int", nullable: false),
                    BusImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromCityId = table.Column<int>(type: "int", nullable: false),
                    ToCityId = table.Column<int>(type: "int", nullable: false),
                    TravelDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartmentTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    PickupLocationId = table.Column<int>(type: "int", nullable: false),
                    DropLocationId = table.Column<int>(type: "int", nullable: false),
                    Fare = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelScheduleTbl", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_TravelScheduleTbl_MasterTbl_BusTypeId",
                        column: x => x.BusTypeId,
                        principalTable: "MasterTbl",
                        principalColumn: "MasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelScheduleTbl_MasterTbl_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "MasterTbl",
                        principalColumn: "MasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelScheduleTbl_MasterTbl_DropLocationId",
                        column: x => x.DropLocationId,
                        principalTable: "MasterTbl",
                        principalColumn: "MasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelScheduleTbl_MasterTbl_FromCityId",
                        column: x => x.FromCityId,
                        principalTable: "MasterTbl",
                        principalColumn: "MasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelScheduleTbl_MasterTbl_PickupLocationId",
                        column: x => x.PickupLocationId,
                        principalTable: "MasterTbl",
                        principalColumn: "MasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelScheduleTbl_MasterTbl_ToCityId",
                        column: x => x.ToCityId,
                        principalTable: "MasterTbl",
                        principalColumn: "MasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelScheduleTbl_UserTbl_PartnerId",
                        column: x => x.PartnerId,
                        principalTable: "UserTbl",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingTbl",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalSeats = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    BoolingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingReference = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTbl", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_BookingTbl_TravelScheduleTbl_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "TravelScheduleTbl",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingTbl_UserTbl_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "UserTbl",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookingPassengerTbl",
                columns: table => new
                {
                    PassengerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    PassengerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPassengerTbl", x => x.PassengerId);
                    table.ForeignKey(
                        name: "FK_BookingPassengerTbl_BookingTbl_BookingId",
                        column: x => x.BookingId,
                        principalTable: "BookingTbl",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingSeatTbl",
                columns: table => new
                {
                    BookingSeatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    PassengerId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeatType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatPrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingSeatTbl", x => x.BookingSeatId);
                    table.ForeignKey(
                        name: "FK_BookingSeatTbl_BookingPassengerTbl_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "BookingPassengerTbl",
                        principalColumn: "PassengerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingSeatTbl_BookingTbl_BookingId",
                        column: x => x.BookingId,
                        principalTable: "BookingTbl",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingSeatTbl_TravelScheduleTbl_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "TravelScheduleTbl",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingPassengerTbl_BookingId",
                table: "BookingPassengerTbl",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeatTbl_BookingId",
                table: "BookingSeatTbl",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeatTbl_PassengerId",
                table: "BookingSeatTbl",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingSeatTbl_ScheduleId_SeatNumber",
                table: "BookingSeatTbl",
                columns: new[] { "ScheduleId", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingTbl_BookingReference",
                table: "BookingTbl",
                column: "BookingReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingTbl_CustomerId",
                table: "BookingTbl",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingTbl_ScheduleId",
                table: "BookingTbl",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelScheduleTbl_BusNumber_TravelDate",
                table: "TravelScheduleTbl",
                columns: new[] { "BusNumber", "TravelDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TravelScheduleTbl_BusTypeId",
                table: "TravelScheduleTbl",
                column: "BusTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelScheduleTbl_CompanyId",
                table: "TravelScheduleTbl",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelScheduleTbl_DropLocationId",
                table: "TravelScheduleTbl",
                column: "DropLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelScheduleTbl_FromCityId",
                table: "TravelScheduleTbl",
                column: "FromCityId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelScheduleTbl_PartnerId",
                table: "TravelScheduleTbl",
                column: "PartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelScheduleTbl_PickupLocationId",
                table: "TravelScheduleTbl",
                column: "PickupLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelScheduleTbl_ToCityId",
                table: "TravelScheduleTbl",
                column: "ToCityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTbl_Email",
                table: "UserTbl",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingSeatTbl");

            migrationBuilder.DropTable(
                name: "BookingPassengerTbl");

            migrationBuilder.DropTable(
                name: "BookingTbl");

            migrationBuilder.DropTable(
                name: "TravelScheduleTbl");

            migrationBuilder.DropTable(
                name: "MasterTbl");

            migrationBuilder.DropTable(
                name: "UserTbl");
        }
    }
}
