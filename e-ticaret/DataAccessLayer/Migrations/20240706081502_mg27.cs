using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mg27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChartFeatures",
                columns: table => new
                {
                    chartID = table.Column<int>(type: "int", nullable: false),
                    featureID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartFeatures", x => new { x.featureID, x.chartID });
                    table.ForeignKey(
                        name: "FK_ChartFeatures_Charts_chartID",
                        column: x => x.chartID,
                        principalTable: "Charts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChartFeatures_Features_featureID",
                        column: x => x.featureID,
                        principalTable: "Features",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChartFeatures_chartID",
                table: "ChartFeatures",
                column: "chartID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropTable(
			   name: "ChartFeatures");
		}
    }
}
