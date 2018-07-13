using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeoServer.Data.Migrations
{
    public partial class ZonalStatKATO_20180713_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ZonalStatKATO_DayOfYear",
                table: "ZonalStatKATO",
                column: "DayOfYear");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ZonalStatKATO_DayOfYear",
                table: "ZonalStatKATO");
        }
    }
}
