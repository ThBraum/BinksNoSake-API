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
                name: "Timoneiros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    CapitaoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timoneiros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Capitaes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    TimoneiroId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capitaes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Capitaes_Timoneiros_TimoneiroId",
                        column: x => x.TimoneiroId,
                        principalTable: "Timoneiros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Piratas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Funcao = table.Column<string>(type: "TEXT", nullable: true),
                    DataIngressoTripulacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Objetivo = table.Column<string>(type: "TEXT", nullable: true),
                    CapitaoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piratas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Piratas_Capitaes_CapitaoId",
                        column: x => x.CapitaoId,
                        principalTable: "Capitaes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Navios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    PirataId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Navios_Piratas_PirataId",
                        column: x => x.PirataId,
                        principalTable: "Piratas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capitaes_TimoneiroId",
                table: "Capitaes",
                column: "TimoneiroId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Navios_PirataId",
                table: "Navios",
                column: "PirataId");

            migrationBuilder.CreateIndex(
                name: "IX_Piratas_CapitaoId",
                table: "Piratas",
                column: "CapitaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Navios");

            migrationBuilder.DropTable(
                name: "Piratas");

            migrationBuilder.DropTable(
                name: "Capitaes");

            migrationBuilder.DropTable(
                name: "Timoneiros");
        }
    }
}
