using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanguagePlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGrammarYouTubeAndQuizLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GrammarTopicId",
                table: "Quizzes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouTubeUrl",
                table: "GrammarTopics",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_GrammarTopicId",
                table: "Quizzes",
                column: "GrammarTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quizzes_GrammarTopics_GrammarTopicId",
                table: "Quizzes",
                column: "GrammarTopicId",
                principalTable: "GrammarTopics",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quizzes_GrammarTopics_GrammarTopicId",
                table: "Quizzes");

            migrationBuilder.DropIndex(
                name: "IX_Quizzes_GrammarTopicId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "GrammarTopicId",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "YouTubeUrl",
                table: "GrammarTopics");
        }
    }
}
