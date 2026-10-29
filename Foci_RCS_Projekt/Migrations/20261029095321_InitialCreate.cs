using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foci_RCS_Projekt.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Csapatok",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nev = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Varos = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    AlapitasEve = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Csapatok", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Merkozesek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HazaiCsapatId = table.Column<int>(type: "INTEGER", nullable: false),
                    VendegCsapatId = table.Column<int>(type: "INTEGER", nullable: false),
                    Datum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HazaiGol = table.Column<int>(type: "INTEGER", nullable: true),
                    VendegGol = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merkozesek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Merkozesek_Csapatok_HazaiCsapatId",
                        column: x => x.HazaiCsapatId,
                        principalTable: "Csapatok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Merkozesek_Csapatok_VendegCsapatId",
                        column: x => x.VendegCsapatId,
                        principalTable: "Csapatok",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Csapatok_Nev",
                table: "Csapatok",
                column: "Nev",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Merkozesek_HazaiCsapatId_VendegCsapatId",
                table: "Merkozesek",
                columns: new[] { "HazaiCsapatId", "VendegCsapatId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Merkozesek_VendegCsapatId",
                table: "Merkozesek",
                column: "VendegCsapatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Merkozesek");

            migrationBuilder.DropTable(
                name: "Csapatok");
        }
    }
}
