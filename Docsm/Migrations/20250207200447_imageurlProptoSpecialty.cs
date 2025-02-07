using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Docsm.Migrations
{
    /// <inheritdoc />
    public partial class imageurlProptoSpecialty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Specialties",
                type: "nvarchar(388)",
                maxLength: 388,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Specialties");
        }
    }
}
