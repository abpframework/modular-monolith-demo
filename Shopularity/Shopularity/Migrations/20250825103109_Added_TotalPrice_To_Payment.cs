using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopularity.Migrations
{
    /// <inheritdoc />
    public partial class Added_TotalPrice_To_Payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "PaymentPayments",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "PaymentPayments");
        }
    }
}
