using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mg30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesDetails",
                table: "SalesDetails");

            migrationBuilder.AddColumn<int>(
                name: "GroupID",
                table: "SalesDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesDetails",
                table: "SalesDetails",
                columns: new[] { "urunID", "salesID", "FeatureID", "GroupID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesDetails",
                table: "SalesDetails");

            migrationBuilder.DropColumn(
                name: "GroupID",
                table: "SalesDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesDetails",
                table: "SalesDetails",
                columns: new[] { "urunID", "salesID", "FeatureID" });
        }
    }
}
