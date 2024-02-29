using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin_panel.Migrations
{
    public partial class addnewmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    cat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cat_name = table.Column<string>(type: "Varchar(50)", nullable: false),
                    cat_status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.cat_id);
                });

            migrationBuilder.CreateTable(
                name: "SuperMarkets",
                columns: table => new
                {
                    sp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sp_name = table.Column<string>(type: "Varchar(50)", nullable: false),
                    sp_town = table.Column<string>(type: "Varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperMarkets", x => x.sp_id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    p_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    p_name = table.Column<string>(type: "Varchar(50)", nullable: false),
                    p_description = table.Column<string>(type: "Varchar(max)", nullable: false),
                    p_mrp = table.Column<double>(type: "float", nullable: false),
                    p_price = table.Column<double>(type: "float", nullable: false),
                    p_stock = table.Column<string>(type: "Varchar(50)", nullable: false),
                    p_category = table.Column<int>(type: "int", nullable: false),
                    p_supermart = table.Column<int>(type: "int", nullable: false),
                    p_img = table.Column<string>(type: "Varchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.p_id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_p_category",
                        column: x => x.p_category,
                        principalTable: "Categories",
                        principalColumn: "cat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_SuperMarkets_p_supermart",
                        column: x => x.p_supermart,
                        principalTable: "SuperMarkets",
                        principalColumn: "sp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_p_category",
                table: "Products",
                column: "p_category");

            migrationBuilder.CreateIndex(
                name: "IX_Products_p_supermart",
                table: "Products",
                column: "p_supermart");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "SuperMarkets");
        }
    }
}
