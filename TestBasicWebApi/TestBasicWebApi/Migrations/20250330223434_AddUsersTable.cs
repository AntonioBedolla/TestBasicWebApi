using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestBasicWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Usuarios",
                newName: "Email");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Usuarios",
                newName: "Username");
        }
    }
}
