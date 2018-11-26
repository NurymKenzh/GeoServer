using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeoServer.Data.Migrations
{
    public partial class Pasture_20181123_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pasture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    areaGa = table.Column<decimal>(nullable: false),
                    class_id = table.Column<int>(nullable: false),
                    group_id = table.Column<int>(nullable: false),
                    group_lt = table.Column<int>(nullable: false),
                    korm_l = table.Column<decimal>(nullable: false),
                    korm_o = table.Column<decimal>(nullable: false),
                    korm_v = table.Column<decimal>(nullable: false),
                    korm_z = table.Column<decimal>(nullable: false),
                    note = table.Column<string>(nullable: true),
                    otdely_id = table.Column<int>(nullable: false),
                    pid = table.Column<int>(nullable: false),
                    recom_id = table.Column<int>(nullable: false),
                    subtype_id = table.Column<int>(nullable: false),
                    ur_l = table.Column<decimal>(nullable: false),
                    ur_o = table.Column<decimal>(nullable: false),
                    ur_v = table.Column<decimal>(nullable: false),
                    ur_z = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasture", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pasture");
        }
    }
}
