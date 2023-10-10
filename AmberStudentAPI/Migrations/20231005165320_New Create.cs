using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmberStudentAPI.Models
{
    public partial class NewCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentIdImageFilePath",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentIdImageFilePath",
                table: "Student");
        }
    }
}
