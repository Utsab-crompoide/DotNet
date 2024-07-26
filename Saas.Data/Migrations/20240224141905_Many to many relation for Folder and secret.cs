using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class ManytomanyrelationforFolderandsecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Secrets_Folders_FolderId",
                table: "Secrets");

            migrationBuilder.DropIndex(
                name: "IX_Secrets_FolderId",
                table: "Secrets");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Secrets");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "OrganizationUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrganizationUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "OrganizationUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedBy",
                table: "OrganizationUsers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "FolderSecrets",
                columns: table => new
                {
                    FolderId = table.Column<Guid>(type: "uuid", nullable: false),
                    SecretId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderSecrets", x => new { x.FolderId, x.SecretId });
                    table.ForeignKey(
                        name: "FK_FolderSecrets_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FolderSecrets_Secrets_SecretId",
                        column: x => x.SecretId,
                        principalTable: "Secrets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolderSecrets_SecretId",
                table: "FolderSecrets",
                column: "SecretId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderSecrets");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OrganizationUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrganizationUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "OrganizationUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "OrganizationUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                table: "Secrets",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Secrets_FolderId",
                table: "Secrets",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Secrets_Folders_FolderId",
                table: "Secrets",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
