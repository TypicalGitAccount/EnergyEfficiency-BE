using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnergyEfficiencyBE.Migrations
{
    /// <inheritdoc />
    public partial class PeakEnergyEfficiencyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeakEnergyConsumptionTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BuildingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoriesMin = table.Column<int>(type: "int", nullable: false),
                    StoriesMax = table.Column<int>(type: "int", nullable: false),
                    TemperatureZone = table.Column<int>(type: "int", nullable: false),
                    Formula = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeakEnergyConsumptionTable", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeakEnergyConsumptionTable");
        }
    }
}
