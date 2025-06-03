using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myapp.Migrations
{
    /// <inheritdoc />
    public partial class addOtpVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordResetTokenExpiry",
                table: "Users",
                newName: "PasswordResetOtpExpiry");

            migrationBuilder.RenameColumn(
                name: "PasswordResetToken",
                table: "Users",
                newName: "PasswordResetOtp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordResetOtpExpiry",
                table: "Users",
                newName: "PasswordResetTokenExpiry");

            migrationBuilder.RenameColumn(
                name: "PasswordResetOtp",
                table: "Users",
                newName: "PasswordResetToken");
        }
    }
}
