using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin_panel.Migrations
{
    public partial class addorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Order_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Order_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Shipping_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Order_total = table.Column<double>(type: "float", nullable: true),
                    Order_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payment_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tracking_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Paymentdue_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Session_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Payment_intentid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    first_name = table.Column<string>(type: "Varchar(50)", nullable: false),
                    last_name = table.Column<string>(type: "Varchar(50)", nullable: false),
                    Town = table.Column<string>(type: "Varchar(50)", nullable: false),
                    City = table.Column<string>(type: "Varchar(50)", nullable: false),
                    Address = table.Column<string>(type: "Varchar(max)", nullable: false),
                    Pcode = table.Column<string>(type: "Varchar(50)", nullable: false),
                    Contact_No = table.Column<string>(type: "Varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Order_id);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_user_id",
                table: "Orders",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
