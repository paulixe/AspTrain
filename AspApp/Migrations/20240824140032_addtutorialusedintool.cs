using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspApp.Migrations
{
    /// <inheritdoc />
    public partial class addtutorialusedintool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Tutorials_TutorialId",
                table: "Tools");

            migrationBuilder.DropIndex(
                name: "IX_Tools_TutorialId",
                table: "Tools");

            migrationBuilder.DropColumn(
                name: "TutorialId",
                table: "Tools");

            migrationBuilder.CreateTable(
                name: "ToolTutorial",
                columns: table => new
                {
                    RequiredToolsId = table.Column<int>(type: "int", nullable: false),
                    UsedInTutorialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolTutorial", x => new { x.RequiredToolsId, x.UsedInTutorialsId });
                    table.ForeignKey(
                        name: "FK_ToolTutorial_Tools_RequiredToolsId",
                        column: x => x.RequiredToolsId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToolTutorial_Tutorials_UsedInTutorialsId",
                        column: x => x.UsedInTutorialsId,
                        principalTable: "Tutorials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToolTutorial_UsedInTutorialsId",
                table: "ToolTutorial",
                column: "UsedInTutorialsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToolTutorial");

            migrationBuilder.AddColumn<int>(
                name: "TutorialId",
                table: "Tools",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 1,
                column: "TutorialId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 2,
                column: "TutorialId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 3,
                column: "TutorialId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Tools_TutorialId",
                table: "Tools",
                column: "TutorialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Tutorials_TutorialId",
                table: "Tools",
                column: "TutorialId",
                principalTable: "Tutorials",
                principalColumn: "Id");
        }
    }
}
