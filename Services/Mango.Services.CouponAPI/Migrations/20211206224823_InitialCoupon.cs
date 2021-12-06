using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.CouponAPI.Migrations
{
    public partial class InitialCoupon : Migration
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
                    { 1, "10OFF", 10.0, new Guid("d40064cf-2d4b-4afb-9883-c0f75841f54c") },
                    { 2, "20OFF", 20.0, new Guid("34efd893-9480-4e49-94f3-42a6a422342d") },
                    { 3, "30OFF", 30.0, new Guid("74caf3f9-a81a-493b-b158-621f1d580214") },
                    { 4, "40OFF", 40.0, new Guid("75869737-dd15-4910-853f-7b1b7c31398f") },
                    { 5, "50OFF", 50.0, new Guid("18b3e267-6868-4f7a-b1c6-e420255b8d12") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
