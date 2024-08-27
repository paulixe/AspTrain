using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspApp.Migrations
{
    /// <inheritdoc />
    public partial class addworkshoplisttotool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
