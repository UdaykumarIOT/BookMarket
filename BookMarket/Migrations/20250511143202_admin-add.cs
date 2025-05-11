using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMarket.Migrations
{
    /// <inheritdoc />
    public partial class adminadd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "100", 0, null, "7a753faa-52e5-47fa-9209-7959229e81f6", new DateTime(2025, 5, 11, 14, 32, 0, 326, DateTimeKind.Utc).AddTicks(8071), "admin@bookmarket.com", true, null, null, false, null, "ADMIN@BOOKMARKET.COM", "ADMIN@BOOKMARKET.COM", "AQAAAAIAAYagAAAAELGfNCF4ihOU5kaAbsFazVOlZaAPtG86UcWf7EiLxFfPAai3FJzwNEE/HFbipNK1cA==", null, false, "12345678-90ab-cdef-1234-567890abcdef", false, "admin@bookmarket.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "100" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "100" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "100");
        }
    }
}
