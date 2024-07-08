using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soft_Eng_Spring2024.Migrations
{
    /// <inheritdoc />
    public partial class Voters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Voters",
                table: "Poll",
                type: "TEXT",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Voters",
                table: "Poll");
        }
    }
}
