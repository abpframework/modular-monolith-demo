using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopularity.Migrations
{
    /// <inheritdoc />
    public partial class Added_CmsKit_Ratings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CmsRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EntityType = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    StarCount = table.Column<short>(type: "smallint", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CmsRatings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CmsRatings_TenantId_EntityType_EntityId_CreatorId",
                table: "CmsRatings",
                columns: new[] { "TenantId", "EntityType", "EntityId", "CreatorId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CmsRatings");
        }
    }
}
