using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStore.Migrations
{
    public partial class AddProductsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "DisplayComments", "ImageSource", "Name", "Price" },
                values: new object[] { 1, 3, "Corn, Zea mays, is an annual grass in the family Poaceae and is a staple food crop grown all over the world. The corn plant possesses a simple stem of nodes and internodes.", true, null, "Corn", 5.99m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "DisplayComments", "ImageSource", "Name", "Price" },
                values: new object[] { 2, 3, "An apple is a sweet, edible fruit produced by an apple tree (Malus domestica).", true, null, "Apple", 2.55m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "DisplayComments", "ImageSource", "Name", "Price" },
                values: new object[] { 3, 2, "A cup is an open-top container used to hold liquids for pouring or drinking; it also can be used to store solids for pouring (e.g., sugar, flour, grains).", true, null, "Cup", 7.50m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
