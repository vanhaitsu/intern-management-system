using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Interviews",
                newName: "InternName");

            migrationBuilder.AddColumn<string>(
                name: "InternEmail",
                table: "Interviews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternEmail",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "InternName",
                table: "Interviews",
                newName: "Name");
        }
    }
}
