using System;
using DataAccessLayer.MigrationExtensions;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherEntryModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperature = table.Column<int>(type: "int", nullable: false),
                    WindSpeed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherEntryModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherEntryModel_LocationModel_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherEntryModel_LocationId",
                table: "WeatherEntryModel",
                column: "LocationId");

            // 12 initial locations
            migrationBuilder.CreateLocation(524901, "Moscow", "Russia");
            migrationBuilder.CreateLocation(2643743, "London", "Great Britain");
            migrationBuilder.CreateLocation(2643736, "Derry", "Great Britain");
            migrationBuilder.CreateLocation(524809, "Mostovskoy", "Russia");
            migrationBuilder.CreateLocation(3202058, "Destrnik", "Slovenia");
            migrationBuilder.CreateLocation(3186557, "Žetale", "Slovenia");
            migrationBuilder.CreateLocation(5379566, "Orangevale", "USA");
            migrationBuilder.CreateLocation(5385793, "Ramona", "USA");
            migrationBuilder.CreateLocation(6552025, "Hörup", "Germany");
            migrationBuilder.CreateLocation(6552023, "Harrislee", "Germany");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherEntryModel");

            migrationBuilder.DropTable(
                name: "LocationModel");
        }
    }
}
