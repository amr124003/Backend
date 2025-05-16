using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace myapp.Migrations
{
    /// <inheritdoc />
    public partial class changePlansOptionTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanOption_Plans_PlanId",
                table: "PlanOption");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanOption",
                table: "PlanOption");

            migrationBuilder.RenameTable(
                name: "PlanOption",
                newName: "PlanOptions");

            migrationBuilder.RenameIndex(
                name: "IX_PlanOption_PlanId",
                table: "PlanOptions",
                newName: "IX_PlanOptions_PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanOptions",
                table: "PlanOptions",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Plans",
                columns: new[] { "Id", "Description", "Features", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, "Remove Add & Unlock All Location", null, "1", "Get Basic" },
                    { 2, "Remove Add & Unlock All Location", null, "crown", "Get Premium" },
                    { 3, "Remove Add & Unlock All Location", null, "2", "Get Basic" }
                });

            migrationBuilder.InsertData(
                table: "PlanOptions",
                columns: new[] { "Id", "DurationMonths", "Label", "Note", "PlanId", "Price", "PriceUnit" },
                values: new object[,]
                {
                    { 1, 1, "1 Month", "that's Basic Plan", 1, 0m, "EGP" },
                    { 2, 6, "6 Month", "that's Premium Plan", 1, 310m, "EGP" },
                    { 3, 12, "1 Year", "that's Premium Plan", 1, 620m, "EGP" },
                    { 4, 1, "1 Month", "that's Basic Plan", 2, 0m, "EGP" },
                    { 5, 6, "6 Month", "that's Premium Plan", 2, 310m, "EGP" },
                    { 6, 12, "1 Year", "that's Premium Plan", 2, 620m, "EGP" },
                    { 7, 1, "1 Month", "that's Basic Plan", 3, 0m, "EGP" },
                    { 8, 6, "6 Month", "that's Premium Plan", 3, 310m, "EGP" },
                    { 9, 12, "1 Year", "that's Premium Plan", 3, 620m, "EGP" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PlanOptions_Plans_PlanId",
                table: "PlanOptions",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanOptions_Plans_PlanId",
                table: "PlanOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanOptions",
                table: "PlanOptions");

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "PlanOptions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "PlanOptions",
                newName: "PlanOption");

            migrationBuilder.RenameIndex(
                name: "IX_PlanOptions_PlanId",
                table: "PlanOption",
                newName: "IX_PlanOption_PlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanOption",
                table: "PlanOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanOption_Plans_PlanId",
                table: "PlanOption",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
