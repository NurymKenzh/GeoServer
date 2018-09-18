using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeoServer.Data.Migrations
{
    public partial class PasXXXXX_20180918_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "PasSubtype",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "PasRecom",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "PasOtdel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "PasGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "PasClass",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "PasSubtype");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PasRecom");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PasOtdel");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PasGroup");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PasClass");
        }
    }
}
