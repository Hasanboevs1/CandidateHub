using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CandidateHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    BestCallTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LinkedInProfile = table.Column<string>(type: "TEXT", nullable: true),
                    GitHubProfile = table.Column<string>(type: "TEXT", nullable: true),
                    Comment = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Candidates",
                columns: new[] { "Id", "BestCallTime", "Comment", "Email", "FirstName", "GitHubProfile", "LastName", "LinkedInProfile", "PhoneNumber" },
                values: new object[] { 1L, new DateTime(2025, 3, 2, 5, 17, 12, 320, DateTimeKind.Utc).AddTicks(7493), "I am open to Work", "hasanboevs@icloud.com", "Muhammadyusuf", "https://github.com/hasanboevs1", "Hasanboyev", "https://uz.linkedin.com/in/hasanboevs", "+998975949511" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidates");
        }
    }
}
