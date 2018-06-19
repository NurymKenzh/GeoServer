using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GeoServer.Data.Migrations
{
    public partial class ModisDataSet_20180619_00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModisDataSet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DataType = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FillValue = table.Column<string>(nullable: true),
                    Index = table.Column<int>(nullable: false),
                    ModisProductId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ScalingFactor = table.Column<string>(nullable: true),
                    Units = table.Column<string>(nullable: true),
                    ValidRange = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModisDataSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModisDataSet_ModisProduct_ModisProductId",
                        column: x => x.ModisProductId,
                        principalTable: "ModisProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModisDataSet_ModisProductId",
                table: "ModisDataSet",
                column: "ModisProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModisDataSet");
        }
    }
}
