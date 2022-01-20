using System;
using DataAccessLayer.Enums;
using DataAccessLayer.MigrationExtensions;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace DataAccessLayer.Migrations
{
    public partial class createdPivot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherAttributeTypeModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherAttributeTypeModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherAttributeModel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherAttributeModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherAttributeModel_LocationModel_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeatherAttributeModel_WeatherAttributeTypeModel_TypeId",
                        column: x => x.TypeId,
                        principalTable: "WeatherAttributeTypeModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAttributeModel_LocationId",
                table: "WeatherAttributeModel",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherAttributeModel_TypeId",
                table: "WeatherAttributeModel",
                column: "TypeId");

            migrationBuilder.SetIdentity<WeatherAttributeTypeModel>(true);
            var enumNames = Enum.GetNames(typeof(AttributeType));
            var enumValues = Enum.GetValues(typeof(AttributeType)).Cast<int>().ToArray();
            for(var i = 0; i < enumNames.Length; i++)
            {
                migrationBuilder.CreateWeatherType(enumValues[i], enumNames[i]);
            }
            migrationBuilder.SetIdentity<WeatherAttributeTypeModel>(false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherAttributeModel");

            migrationBuilder.DropTable(
                name: "WeatherAttributeTypeModel");
        }
    }
}
