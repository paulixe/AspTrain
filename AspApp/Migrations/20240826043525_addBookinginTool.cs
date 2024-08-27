using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspApp.Migrations
{
    /// <inheritdoc />
    public partial class addBookinginTool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Bookings_BookingId",
                table: "Tools");

            migrationBuilder.DropIndex(
                name: "IX_Tools_BookingId",
                table: "Tools");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Tools");

            migrationBuilder.CreateTable(
                name: "BookingTool",
                columns: table => new
                {
                    BookingsId = table.Column<int>(type: "int", nullable: false),
                    ItemsBookedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingTool", x => new { x.BookingsId, x.ItemsBookedId });
                    table.ForeignKey(
                        name: "FK_BookingTool_Bookings_BookingsId",
                        column: x => x.BookingsId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingTool_Tools_ItemsBookedId",
                        column: x => x.ItemsBookedId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingTool_ItemsBookedId",
                table: "BookingTool",
                column: "ItemsBookedId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingTool");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Tools",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 1,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 2,
                column: "BookingId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tools",
                keyColumn: "Id",
                keyValue: 3,
                column: "BookingId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Tools_BookingId",
                table: "Tools",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Bookings_BookingId",
                table: "Tools",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");
        }
    }
}
