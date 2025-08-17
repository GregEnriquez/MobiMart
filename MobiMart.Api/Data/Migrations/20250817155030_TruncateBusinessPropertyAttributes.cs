using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobiMart.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class TruncateBusinessPropertyAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BusinessName",
                table: "Businesses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BusinessCode",
                table: "Businesses",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "BusinessAddress",
                table: "Businesses",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Businesses",
                newName: "BusinessName");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Businesses",
                newName: "BusinessCode");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Businesses",
                newName: "BusinessAddress");
        }
    }
}
