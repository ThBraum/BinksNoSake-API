using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BinksNoSake.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Capitaes",
                columns: table => new
                {
                    CapitaoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    TimoneiroId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capitaes", x => x.CapitaoId);
                });

            migrationBuilder.CreateTable(
                name: "Piratas",
                columns: table => new
                {
                    PirataId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Funcao = table.Column<string>(type: "TEXT", nullable: true),
                    DataIngressoTripulacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Objetivo = table.Column<string>(type: "TEXT", nullable: true),
                    CapitaoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piratas", x => x.PirataId);
                    table.ForeignKey(
                        name: "FK_Piratas_Capitaes_CapitaoId",
                        column: x => x.CapitaoId,
                        principalTable: "Capitaes",
                        principalColumn: "CapitaoId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Timoneiros",
                columns: table => new
                {
                    TimoneiroId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    CapitaoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timoneiros", x => x.TimoneiroId);
                    table.ForeignKey(
                        name: "FK_Timoneiros_Capitaes_CapitaoId",
                        column: x => x.CapitaoId,
                        principalTable: "Capitaes",
                        principalColumn: "CapitaoId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Navios",
                columns: table => new
                {
                    NavioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    PirataId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navios", x => x.NavioId);
                    table.ForeignKey(
                        name: "FK_Navios_Piratas_PirataId",
                        column: x => x.PirataId,
                        principalTable: "Piratas",
                        principalColumn: "PirataId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Navios_PirataId",
                table: "Navios",
                column: "PirataId");

            migrationBuilder.CreateIndex(
                name: "IX_Piratas_CapitaoId",
                table: "Piratas",
                column: "CapitaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Timoneiros_CapitaoId",
                table: "Timoneiros",
                column: "CapitaoId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Navios");

            migrationBuilder.DropTable(
                name: "Timoneiros");

            migrationBuilder.DropTable(
                name: "Piratas");

            migrationBuilder.DropTable(
                name: "Capitaes");
        }
    }
}
