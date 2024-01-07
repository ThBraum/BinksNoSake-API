using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BinksNoSake.Persistence.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UltimoNome",
                table: "AspNetUsers",
                newName: "Sobrenome");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sobrenome",
                table: "AspNetUsers",
                newName: "UltimoNome");
        }
    }
}
