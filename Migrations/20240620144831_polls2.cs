using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Soft_Eng_Spring2024.Migrations
{
    /// <inheritdoc />
    public partial class polls2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Votes",
                table: "Poll",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Poll");
        }
    }
}
