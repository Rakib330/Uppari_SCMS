using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Uppari_SCMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VendorName",
                table: "Vendors",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Vendors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderName",
                table: "PurchaseOrders",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrderId",
                table: "PurchaseOrders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "Items",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Items",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Vendors",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Vendors");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Vendors",
                newName: "VendorName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vendors",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PurchaseOrders",
                newName: "PurchaseOrderName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PurchaseOrders",
                newName: "PurchaseOrderId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Items",
                newName: "ItemName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Items",
                newName: "ItemId");
        }
    }
}
