using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CustomerOnboarding.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lgas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lgas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lgas_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPhoneVerified = table.Column<bool>(type: "bit", nullable: false),
                    OnboardingCompleted = table.Column<bool>(type: "bit", nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LgaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PendingOtp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtpExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Lgas_LgaId",
                        column: x => x.LgaId,
                        principalTable: "Lgas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("a05288f7-a58c-4af4-bd22-c2e2532f952b"), "Lagos" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { new Guid("6f5c1ca5-5f78-4d15-b573-1c2109054fcf"), "$2a$11$Qz3LfrWfEHF5r6NhaaTkS.csRaL2KeAI2RYmkebUaaNG49bYgn.RW", "User", "user" },
                    { new Guid("b4a2301e-d019-489f-97ed-ed3eacbec1c7"), "$2a$11$09kqxVNLmgHxNAXJgg84weALfO2n.Hglevlz7Ddr..3CwtSoCzJWm", "Administrator", "admin" }
                });

            migrationBuilder.InsertData(
                table: "Lgas",
                columns: new[] { "Id", "Name", "StateId" },
                values: new object[,]
                {
                    { new Guid("05b1237e-b189-4c90-adcf-785047b76362"), "Lagos-Island", new Guid("a05288f7-a58c-4af4-bd22-c2e2532f952b") },
                    { new Guid("12c0f8cb-3b7f-4072-9061-ca2f3a3f2d90"), "Ikeja", new Guid("a05288f7-a58c-4af4-bd22-c2e2532f952b") },
                    { new Guid("14597d69-f037-49bf-97c1-20257aeeacd7"), "Surulere", new Guid("a05288f7-a58c-4af4-bd22-c2e2532f952b") },
                    { new Guid("6dc08fa6-2b77-49d0-b3e2-1e3cd58cbcaf"), "Lagos-Mainland", new Guid("a05288f7-a58c-4af4-bd22-c2e2532f952b") },
                    { new Guid("9d454989-b9b2-42fc-b3d4-42748afdef11"), "Alimosho", new Guid("a05288f7-a58c-4af4-bd22-c2e2532f952b") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_LgaId",
                table: "Customers",
                column: "LgaId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_StateId",
                table: "Customers",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Lgas_StateId",
                table: "Lgas",
                column: "StateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Lgas");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
