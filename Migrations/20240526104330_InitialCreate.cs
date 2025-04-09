using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PiwkoMozna.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrowarModel",
                columns: table => new
                {
                    BreweryName = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    Founded = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrowarModel", x => x.BreweryName);
                });

            migrationBuilder.CreateTable(
                name: "UzytkownikModel",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UzytkownikModel", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "PiwoModel",
                columns: table => new
                {
                    BeerName = table.Column<string>(type: "TEXT", nullable: false),
                    BreweryName = table.Column<string>(type: "TEXT", nullable: false),
                    Style = table.Column<string>(type: "TEXT", nullable: false),
                    ABV = table.Column<double>(type: "REAL", nullable: false),
                    AverageRating = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PiwoModel", x => x.BeerName);
                    table.ForeignKey(
                        name: "FK_PiwoModel_BrowarModel_BreweryName",
                        column: x => x.BreweryName,
                        principalTable: "BrowarModel",
                        principalColumn: "BreweryName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecenzjaModel",
                columns: table => new
                {
                    ReviewID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    BeerName = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecenzjaModel", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_RecenzjaModel_PiwoModel_BeerName",
                        column: x => x.BeerName,
                        principalTable: "PiwoModel",
                        principalColumn: "BeerName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecenzjaModel_UzytkownikModel_UserID",
                        column: x => x.UserID,
                        principalTable: "UzytkownikModel",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PiwoModel_BreweryName",
                table: "PiwoModel",
                column: "BreweryName");

            migrationBuilder.CreateIndex(
                name: "IX_RecenzjaModel_BeerName",
                table: "RecenzjaModel",
                column: "BeerName");

            migrationBuilder.CreateIndex(
                name: "IX_RecenzjaModel_UserID",
                table: "RecenzjaModel",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecenzjaModel");

            migrationBuilder.DropTable(
                name: "PiwoModel");

            migrationBuilder.DropTable(
                name: "UzytkownikModel");

            migrationBuilder.DropTable(
                name: "BrowarModel");
        }
    }
}
