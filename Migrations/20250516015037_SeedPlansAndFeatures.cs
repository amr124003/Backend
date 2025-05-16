using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace myapp.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlansAndFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanOptions_Plans_PlanId",
                table: "PlanOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlanOptions",
                table: "PlanOptions");

            migrationBuilder.DropColumn(
                name: "Features",
                table: "Plans");

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

            migrationBuilder.CreateTable(
                name: "PlanFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanFeatures_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PlanFeatures",
                columns: new[] { "Id", "Description", "PlanId" },
                values: new object[,]
                {
                    { 1, "Limited VR Training", 1 },
                    { 2, "Access to Home Scenario", 1 },
                    { 3, "AI Chatbot Support", 2 },
                    { 4, "Home & Factory Scenarios", 2 },
                    { 5, "Full VR Training Access", 2 },
                    { 6, "Certification", 2 },
                    { 7, "Unlimited VR Training", 3 },
                    { 8, "All Scenarios (Home, Factory, Vehicle)", 3 },
                    { 9, "AI Chatbot + Burn Detection", 3 },
                    { 10, "Multi-User & Custom Reports", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeatures_PlanId",
                table: "PlanFeatures",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanOption_Plans_PlanId",
                table: "PlanOption",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanOption_Plans_PlanId",
                table: "PlanOption");

            migrationBuilder.DropTable(
                name: "PlanFeatures");

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

            migrationBuilder.AddColumn<string>(
                name: "Features",
                table: "Plans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlanOptions",
                table: "PlanOptions",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 1,
                column: "Features",
                value: null);

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 2,
                column: "Features",
                value: null);

            migrationBuilder.UpdateData(
                table: "Plans",
                keyColumn: "Id",
                keyValue: 3,
                column: "Features",
                value: null);

            migrationBuilder.AddForeignKey(
                name: "FK_PlanOptions_Plans_PlanId",
                table: "PlanOptions",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
