using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmailVerifyCode",
                table: "Interns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredCode",
                table: "Interns",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "Interns",
                type: "tinyint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerifyCode",
                table: "Interns");

            migrationBuilder.DropColumn(
                name: "ExpiredCode",
                table: "Interns");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Interns");
        }
    }
}
