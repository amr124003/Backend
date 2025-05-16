using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace myapp.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanOptionIdToPlanFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanFeatures_Plans_PlanId",
                table: "PlanFeatures");

            migrationBuilder.RenameColumn(
                name: "PlanId",
                table: "PlanFeatures",
                newName: "PlanOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_PlanFeatures_PlanId",
                table: "PlanFeatures",
                newName: "IX_PlanFeatures_PlanOptionId");

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "PlanOptionId" },
                values: new object[] { "Limited VR Training", 4 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "PlanOptionId" },
                values: new object[] { "Access to Home Scenario", 4 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "PlanOptionId" },
                values: new object[] { "Limited VR Training", 7 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "PlanOptionId" },
                values: new object[] { "Access to Home Scenario", 7 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "PlanOptionId" },
                values: new object[] { "AI Chatbot Support", 2 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "PlanOptionId" },
                values: new object[] { "Home & Factory Scenarios", 2 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "PlanOptionId" },
                values: new object[] { "Full VR Training Access", 2 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "PlanOptionId" },
                values: new object[] { "Certification", 2 });

            migrationBuilder.InsertData(
                table: "PlanFeatures",
                columns: new[] { "Id", "Description", "PlanOptionId" },
                values: new object[,]
                {
                    { 11, "AI Chatbot Support", 5 },
                    { 12, "Home & Factory Scenarios", 5 },
                    { 13, "Full VR Training Access", 5 },
                    { 14, "Certification", 5 },
                    { 15, "AI Chatbot Support", 8 },
                    { 16, "Home & Factory Scenarios", 8 },
                    { 17, "Full VR Training Access", 8 },
                    { 18, "Certification", 8 },
                    { 19, "Unlimited VR Training", 3 },
                    { 20, "All Scenarios (Home, Factory, Vehicle)", 3 },
                    { 21, "AI Chatbot + Burn Detection", 3 },
                    { 22, "Multi-User & Custom Reports", 3 },
                    { 23, "Unlimited VR Training", 6 },
                    { 24, "All Scenarios (Home, Factory, Vehicle)", 6 },
                    { 25, "AI Chatbot + Burn Detection", 6 },
                    { 26, "Multi-User & Custom Reports", 6 },
                    { 27, "Unlimited VR Training", 9 },
                    { 28, "All Scenarios (Home, Factory, Vehicle)", 9 },
                    { 29, "AI Chatbot + Burn Detection", 9 },
                    { 30, "Multi-User & Custom Reports", 9 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFeatures_PlanOption_PlanOptionId",
                table: "PlanFeatures",
                column: "PlanOptionId",
                principalTable: "PlanOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanFeatures_PlanOption_PlanOptionId",
                table: "PlanFeatures");

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.RenameColumn(
                name: "PlanOptionId",
                table: "PlanFeatures",
                newName: "PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_PlanFeatures_PlanOptionId",
                table: "PlanFeatures",
                newName: "IX_PlanFeatures_PlanId");

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "PlanId" },
                values: new object[] { "AI Chatbot Support", 2 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "PlanId" },
                values: new object[] { "Home & Factory Scenarios", 2 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "PlanId" },
                values: new object[] { "Full VR Training Access", 2 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "PlanId" },
                values: new object[] { "Certification", 2 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Description", "PlanId" },
                values: new object[] { "Unlimited VR Training", 3 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Description", "PlanId" },
                values: new object[] { "All Scenarios (Home, Factory, Vehicle)", 3 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Description", "PlanId" },
                values: new object[] { "AI Chatbot + Burn Detection", 3 });

            migrationBuilder.UpdateData(
                table: "PlanFeatures",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Description", "PlanId" },
                values: new object[] { "Multi-User & Custom Reports", 3 });

            migrationBuilder.AddForeignKey(
                name: "FK_PlanFeatures_Plans_PlanId",
                table: "PlanFeatures",
                column: "PlanId",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
