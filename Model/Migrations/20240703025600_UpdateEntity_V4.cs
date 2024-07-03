using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntity_V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPrograms_Assignments_AssignmentId",
                table: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPrograms_AssignmentId",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "TrainingPrograms");

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingProgramId",
                table: "Assignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TrainingProgramId",
                table: "Assignments",
                column: "TrainingProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_TrainingPrograms_TrainingProgramId",
                table: "Assignments",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_TrainingPrograms_TrainingProgramId",
                table: "Assignments");

            migrationBuilder.DropIndex(
                name: "IX_Assignments_TrainingProgramId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "TrainingProgramId",
                table: "Assignments");

            migrationBuilder.AddColumn<Guid>(
                name: "AssignmentId",
                table: "TrainingPrograms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_AssignmentId",
                table: "TrainingPrograms",
                column: "AssignmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPrograms_Assignments_AssignmentId",
                table: "TrainingPrograms",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id");
        }
    }
}
