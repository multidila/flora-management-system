using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FloraManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flowers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Soil = table.Column<int>(type: "integer", nullable: false),
                    Origin = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StemColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LeafColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AverageSize = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TemperatureCelsius = table.Column<int>(type: "integer", nullable: false),
                    IsPhotophilous = table.Column<bool>(type: "boolean", nullable: false),
                    WateringPerWeek = table.Column<int>(type: "integer", nullable: false),
                    Multiplying = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flowers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flowers");
        }
    }
}
