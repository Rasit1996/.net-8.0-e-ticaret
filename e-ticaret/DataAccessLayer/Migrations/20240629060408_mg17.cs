using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mg17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_UserID",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Messages",
                newName: "SendUserID");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_UserID",
                table: "Messages",
                newName: "IX_Messages_SendUserID");

            migrationBuilder.AddColumn<int>(
                name: "ReceiveUserID",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiveUserID",
                table: "Messages",
                column: "ReceiveUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiveUserID",
                table: "Messages",
                column: "ReceiveUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_SendUserID",
                table: "Messages",
                column: "SendUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_ReceiveUserID",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_AspNetUsers_SendUserID",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiveUserID",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReceiveUserID",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "SendUserID",
                table: "Messages",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SendUserID",
                table: "Messages",
                newName: "IX_Messages_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_AspNetUsers_UserID",
                table: "Messages",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
