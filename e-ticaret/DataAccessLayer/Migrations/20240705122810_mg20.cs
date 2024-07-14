using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mg20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_AspNetUsers_UserID",
                table: "Charts");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Charts",
                newName: "userID");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_AspNetUsers_userID",
                table: "Charts",
                column: "userID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Charts_AspNetUsers_userID",
                table: "Charts");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Charts",
                newName: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Charts_AspNetUsers_UserID",
                table: "Charts",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
