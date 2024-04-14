using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin_panel.Migrations
{
    public partial class addcontact2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    c_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    c_name = table.Column<string>(type: "Varchar(50)", nullable: false),
                    c_email = table.Column<string>(type: "Varchar(50)", nullable: false),
                    c_msg = table.Column<string>(type: "Varchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.c_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
