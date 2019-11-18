using Microsoft.EntityFrameworkCore.Migrations;

namespace ErrorIt.Api.Migrations
{
    public partial class updatedDefinitions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationGroups_GroupId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_ErrorTemplates_HttpStatusCode",
                table: "ErrorTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Applications_GroupId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "HttpStatusCode",
                table: "ErrorTemplates");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Applications");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationErrorCode",
                table: "ErrorTemplates",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationGroupId",
                table: "Applications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTemplates_ApplicationErrorCode_ApplicationId",
                table: "ErrorTemplates",
                columns: new[] { "ApplicationErrorCode", "ApplicationId" },
                unique: true,
                filter: "[ApplicationErrorCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationGroupId",
                table: "Applications",
                column: "ApplicationGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationGroups_ApplicationGroupId",
                table: "Applications",
                column: "ApplicationGroupId",
                principalTable: "ApplicationGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ApplicationGroups_ApplicationGroupId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_ErrorTemplates_ApplicationErrorCode_ApplicationId",
                table: "ErrorTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ApplicationGroupId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApplicationErrorCode",
                table: "ErrorTemplates");

            migrationBuilder.DropColumn(
                name: "ApplicationGroupId",
                table: "Applications");

            migrationBuilder.AddColumn<int>(
                name: "HttpStatusCode",
                table: "ErrorTemplates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ErrorTemplates_HttpStatusCode",
                table: "ErrorTemplates",
                column: "HttpStatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_GroupId",
                table: "Applications",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ApplicationGroups_GroupId",
                table: "Applications",
                column: "GroupId",
                principalTable: "ApplicationGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
