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
                    { 1, "10OFF", 10.0, new Guid("770a4868-d099-4b62-ab22-ff62ab4929c7") },
                    { 2, "20OFF", 20.0, new Guid("20315dc3-9772-4343-afb5-646d21ad7c78") },
                    { 3, "30OFF", 30.0, new Guid("1783df47-55b6-44b9-8f84-a4e64318308c") },
                    { 4, "40OFF", 40.0, new Guid("e70dd92d-8c28-4a1c-aeda-a3071d0df91d") },
                    { 5, "50OFF", 50.0, new Guid("92c9e9ec-99c8-453e-9435-303e33b98001") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupons");
        }
    }
}
