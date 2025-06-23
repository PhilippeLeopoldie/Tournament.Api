using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTournamententitytoTournamentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_TournamentDetails_Tournament2Id",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "Tournament2Id",
                table: "Game",
                newName: "TournamentDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_Tournament2Id",
                table: "Game",
                newName: "IX_Game_TournamentDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_TournamentDetails_TournamentDetailsId",
                table: "Game",
                column: "TournamentDetailsId",
                principalTable: "TournamentDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Game_TournamentDetails_TournamentDetailsId",
                table: "Game");

            migrationBuilder.RenameColumn(
                name: "TournamentDetailsId",
                table: "Game",
                newName: "Tournament2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Game_TournamentDetailsId",
                table: "Game",
                newName: "IX_Game_Tournament2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Game_TournamentDetails_Tournament2Id",
                table: "Game",
                column: "Tournament2Id",
                principalTable: "TournamentDetails",
                principalColumn: "Id");
        }
    }
}
