using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguagePlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDictationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Words_WordId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Words_WordId",
                table: "Flashcards");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DictationSets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "DictationSets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AudioTitle",
                table: "DictationSentences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "DictationSentences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Hint",
                table: "DictationSentences",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Words_WordId",
                table: "Favorites",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Words_WordId",
                table: "Flashcards",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Words_WordId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Words_WordId",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "DictationSets");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "DictationSets");

            migrationBuilder.DropColumn(
                name: "AudioTitle",
                table: "DictationSentences");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "DictationSentences");

            migrationBuilder.DropColumn(
                name: "Hint",
                table: "DictationSentences");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Words_WordId",
                table: "Favorites",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Words_WordId",
                table: "Flashcards",
                column: "WordId",
                principalTable: "Words",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
