using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin_panel.Migrations
{
    public partial class changeinproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "p_img",
                table: "Products",
                type: "Varchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Varchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "p_img",
                table: "Products",
                type: "Varchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "Varchar(max)",
                oldNullable: true);
        }
    }
}
