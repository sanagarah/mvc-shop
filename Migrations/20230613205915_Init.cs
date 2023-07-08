using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvc_shop.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Daddies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Sana = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Daddies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Daddies");
        }
    }
}
