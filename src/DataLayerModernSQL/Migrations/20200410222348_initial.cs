using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayerModernSQL.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AzureDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Supported = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ServiceName = table.Column<string>(nullable: true),
                    CloudProvider = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    DetailsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_AzureDetails_DetailsId",
                        column: x => x.DetailsId,
                        principalTable: "AzureDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearningResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ServiceID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Uri = table.Column<string>(nullable: true),
                    ServiceDescriptionId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningResources_Services_ServiceDescriptionId",
                        column: x => x.ServiceDescriptionId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_ServiceDescriptionId",
                table: "LearningResources",
                column: "ServiceDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_DetailsId",
                table: "Services",
                column: "DetailsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningResources");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "AzureDetails");
        }
    }
}
