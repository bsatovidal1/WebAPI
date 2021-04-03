using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project1_BrunoVidal.Data.AWMigrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AW");

            migrationBuilder.CreateTable(
                name: "ArtTypes",
                schema: "AW",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Type = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Artworks",
                schema: "AW",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(maxLength: 256, nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Completed = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 511, nullable: false),
                    Value = table.Column<double>(nullable: false),
                    ArtTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artworks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Artworks_ArtTypes_ArtTypeID",
                        column: x => x.ArtTypeID,
                        principalSchema: "AW",
                        principalTable: "ArtTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_ArtTypeID",
                schema: "AW",
                table: "Artworks",
                column: "ArtTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Artworks_Name_Completed_ArtTypeID",
                schema: "AW",
                table: "Artworks",
                columns: new[] { "Name", "Completed", "ArtTypeID" },
                unique: true);

            ExtraMigration.Steps(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artworks",
                schema: "AW");

            migrationBuilder.DropTable(
                name: "ArtTypes",
                schema: "AW");
        }
    }
}
