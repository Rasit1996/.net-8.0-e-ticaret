﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mg10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uruns_Categories_categoryID",
                table: "Uruns");

            migrationBuilder.AddForeignKey(
                name: "FK_Uruns_Categories_categoryID",
                table: "Uruns",
                column: "categoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uruns_Categories_categoryID",
                table: "Uruns");

            migrationBuilder.AddForeignKey(
                name: "FK_Uruns_Categories_categoryID",
                table: "Uruns",
                column: "categoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
