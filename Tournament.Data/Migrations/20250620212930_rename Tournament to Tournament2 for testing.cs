using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Data.Migrations
{
    /// <inheritdoc />
    public partial class renameTournamenttoTournament2fortesting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_TournamentDetails_TournamentId",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_TournamentId",
                table: "Game");

            migrationBuilder.AddColumn<int>(
                name: "Tournament2Id",
                table: "Game",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_Tournament2Id",
                table: "Game",
                column: "Tournament2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_TournamentDetails_Tournament2Id",
                table: "Game",
                column: "Tournament2Id",
                principalTable: "TournamentDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_TournamentDetails_Tournament2Id",
                table: "Game");

            migrationBuilder.DropIndex(
                name: "IX_Game_Tournament2Id",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Tournament2Id",
                table: "Game");

            migrationBuilder.CreateIndex(
                name: "IX_Game_TournamentId",
                table: "Game",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_TournamentDetails_TournamentId",
                table: "Game",
                column: "TournamentId",
                principalTable: "TournamentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
