using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspApp.Migrations
{
    /// <inheritdoc />
    public partial class specifyManyTOMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workshops_Tools_ToolId",
                table: "Workshops");

            migrationBuilder.DropIndex(
                name: "IX_Workshops_ToolId",
                table: "Workshops");

            migrationBuilder.DropColumn(
                name: "ToolId",
                table: "Workshops");

            migrationBuilder.CreateTable(
                name: "ToolWorkshop",
                columns: table => new
                {
                    AvailableAtWorkshopsId = table.Column<int>(type: "int", nullable: false),
                    ToolsAvailableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToolWorkshop", x => new { x.AvailableAtWorkshopsId, x.ToolsAvailableId });
                    table.ForeignKey(
                        name: "FK_ToolWorkshop_Tools_ToolsAvailableId",
                        column: x => x.ToolsAvailableId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToolWorkshop_Workshops_AvailableAtWorkshopsId",
                        column: x => x.AvailableAtWorkshopsId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToolWorkshop_ToolsAvailableId",
                table: "ToolWorkshop",
                column: "ToolsAvailableId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToolWorkshop");

            migrationBuilder.AddColumn<int>(
                name: "ToolId",
                table: "Workshops",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 1,
                column: "ToolId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Workshops",
                keyColumn: "Id",
                keyValue: 2,
                column: "ToolId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_ToolId",
                table: "Workshops",
                column: "ToolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workshops_Tools_ToolId",
                table: "Workshops",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id");
        }
    }
}
