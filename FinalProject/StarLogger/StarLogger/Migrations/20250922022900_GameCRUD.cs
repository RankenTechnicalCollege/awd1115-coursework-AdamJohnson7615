using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StarLogger.Migrations
{
    /// <inheritdoc />
    public partial class GameCRUD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    AchievementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.AchievementId);
                    table.ForeignKey(
                        name: "FK_Achievements_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "GameId", "Genre", "Platform", "ReleaseDate", "Title" },
                values: new object[,]
                {
                    { 1, "Strategy", "PC", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minesweeper" },
                    { 2, "Platformer", "Nintendo Switch", new DateTime(2017, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Super Mario Odyssey" }
                });

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "AchievementId", "Description", "GameId", "Name", "Points" },
                values: new object[,]
                {
                    { 1, "Click the first square without hitting a mine", 1, "First Click", 10 },
                    { 2, "Successfully clear your first Minesweeper board", 1, "Win First Game", 20 },
                    { 3, "Collect your first Power Moon", 2, "Moon Collector", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_GameId",
                table: "Achievements",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
