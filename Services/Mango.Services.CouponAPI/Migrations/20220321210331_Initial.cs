using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.CouponAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "DiscountAmount", "PublicId" },
                values: new object[,]
                {
                    { 1, "10OFF", 10.0, new Guid("21127ea1-7c7a-47df-b5e5-e83c45ca112b") },
                    { 2, "20OFF", 20.0, new Guid("6109c6ee-0193-4728-ae49-01a447d16831") },
                    { 3, "30OFF", 30.0, new Guid("42de469a-3842-4b6b-8f37-8f7d50213b48") },
                    { 4, "40OFF", 40.0, new Guid("bf84eec2-ec47-4468-b049-1c00b3f317d6") },
                    { 5, "50OFF", 50.0, new Guid("e53c63bd-a2a3-4e7a-b997-907fe0d66aa7") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
