using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddedSideMealBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MainMeal",
                table: "MenuItems",
                newName: "IsSideMeal");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainMeal",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMainMeal",
                table: "MenuItems");

            migrationBuilder.RenameColumn(
                name: "IsSideMeal",
                table: "MenuItems",
                newName: "MainMeal");
        }
    }
}
