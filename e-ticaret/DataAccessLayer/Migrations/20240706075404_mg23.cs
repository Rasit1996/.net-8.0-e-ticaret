using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mg23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Charts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Charts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    count = table.Column<int>(type: "int", nullable: false),
                    urunID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Charts_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Charts_Uruns_urunID",
                        column: x => x.urunID,
                        principalTable: "Uruns",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Charts_AppUserId",
                table: "Charts",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Charts_urunID",
                table: "Charts",
                column: "urunID");
        }
    }
}
