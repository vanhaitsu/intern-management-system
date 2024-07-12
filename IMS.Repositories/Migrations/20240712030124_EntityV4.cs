using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class EntityV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Accounts_AccountId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Interns_InternId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_TrainingPrograms_TrainingProgramId",
                table: "Feedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback");

            migrationBuilder.RenameTable(
                name: "Feedback",
                newName: "Feedbacks");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_TrainingProgramId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_TrainingProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_InternId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_InternId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_AccountId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedbacks",
                table: "Feedbacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Accounts_AccountId",
                table: "Feedbacks",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Interns_InternId",
                table: "Feedbacks",
                column: "InternId",
                principalTable: "Interns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_TrainingPrograms_TrainingProgramId",
                table: "Feedbacks",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Accounts_AccountId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Interns_InternId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_TrainingPrograms_TrainingProgramId",
                table: "Feedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedbacks",
                table: "Feedbacks");

            migrationBuilder.RenameTable(
                name: "Feedbacks",
                newName: "Feedback");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_TrainingProgramId",
                table: "Feedback",
                newName: "IX_Feedback_TrainingProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_InternId",
                table: "Feedback",
                newName: "IX_Feedback_InternId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_AccountId",
                table: "Feedback",
                newName: "IX_Feedback_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Accounts_AccountId",
                table: "Feedback",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Interns_InternId",
                table: "Feedback",
                column: "InternId",
                principalTable: "Interns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_TrainingPrograms_TrainingProgramId",
                table: "Feedback",
                column: "TrainingProgramId",
                principalTable: "TrainingPrograms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
