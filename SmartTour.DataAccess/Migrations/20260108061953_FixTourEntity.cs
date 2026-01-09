using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartTour.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixTourEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    TourType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Raiting = table.Column<double>(type: "float", nullable: false),
                    AviableFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AviableTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CredientAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailedLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AuthProvider = table.Column<int>(type: "int", nullable: false),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tours",
                columns: new[] { "Id", "AviableFrom", "AviableTo", "CredientAt", "DurationDays", "GroupType", "Price", "Raiting", "Region", "Title", "TourType" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 8, 6, 19, 53, 74, DateTimeKind.Utc).AddTicks(1711), 4, "Family", 850m, 4.7000000000000002, "Sheki", "Sheki Cultural Escape", "Cultural" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tours");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
