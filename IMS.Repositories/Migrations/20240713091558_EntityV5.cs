using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Interviews",
                newName: "Date");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "Interviews",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Interviews");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Interviews",
                newName: "DateTime");
        }
    }
}
