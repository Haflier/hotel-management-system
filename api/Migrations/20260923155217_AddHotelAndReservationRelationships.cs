using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelAndReservationRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Order",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Food",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Drink",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "HotelId", "Name", "Price" },
                values: new object[] { "Traditional black tea served hot.", 1, "Persian Tea", 3m });

            migrationBuilder.UpdateData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "HotelId", "Name", "Price" },
                values: new object[] { "Freshly squeezed orange juice.", 1, "Fresh Orange Juice", 5m });

            migrationBuilder.UpdateData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "HotelId", "Name", "Price" },
                values: new object[] { "Fresh pomegranate juice.", 1, "Pomegranate Juice", 6m });

            migrationBuilder.InsertData(
                table: "Drink",
                columns: new[] { "Id", "Description", "HotelId", "Meal", "Name", "Price" },
                values: new object[,]
                {
                    { 4, "Traditional chilled yogurt drink with dried mint.", 1, "Lunch", "Doogh", 4m },
                    { 5, "Chilled Persian saffron and rose water drink.", 1, "Dinner", "Saffron Sherbet", 5m },
                    { 6, "Fresh lemon juice, mint and sparkling water.", 1, "Dinner", "Mint Lemonade", 5m },
                    { 7, "Freshly brewed single espresso.", 1, "Breakfast", "Espresso", 4m },
                    { 8, "Espresso with hot water.", 1, "Breakfast", "Americano", 5m },
                    { 11, "Black tea infused with saffron.", 2, "Breakfast", "Saffron Tea", 4m },
                    { 12, "Freshly prepared pomegranate juice.", 2, "Lunch", "Pomegranate Juice", 6m },
                    { 13, "Cold traditional yogurt drink with mint.", 2, "Lunch", "Doogh", 4m },
                    { 14, "Refreshing juice made from Iranian barberries.", 2, "Lunch", "Barberry Juice", 5m },
                    { 15, "Chilled rose water drink with a delicate floral flavor.", 2, "Dinner", "Rose Water Sherbet", 5m },
                    { 16, "Fresh lemon, mint and sparkling water served chilled.", 2, "Dinner", "Lemon Mint Cooler", 5m },
                    { 17, "Strong traditionally brewed coffee.", 2, "Breakfast", "Turkish Coffee", 4m },
                    { 18, "Espresso with steamed milk and foam.", 2, "Breakfast", "Cappuccino", 6m },
                    { 21, "Traditional Iranian black tea served hot.", 3, "Breakfast", "Persian Black Tea", 3m },
                    { 22, "Refreshing traditional sour cherry juice.", 3, "Lunch", "Sour Cherry Juice", 5m },
                    { 23, "Chilled yogurt drink with mint and a lightly salted flavor.", 3, "Lunch", "Doogh", 4m },
                    { 24, "Fresh pomegranate juice served chilled.", 3, "Lunch", "Pomegranate Juice", 6m },
                    { 25, "Traditional chilled rose water drink.", 3, "Dinner", "Rose Water Sherbet", 5m },
                    { 26, "Fresh lemon drink infused with saffron.", 3, "Dinner", "Saffron Lemonade", 5m },
                    { 27, "Black tea brewed with cardamom and saffron.", 3, "Breakfast", "Persian Cardamom Tea", 4m },
                    { 28, "Strong traditionally brewed coffee served in a small cup.", 3, "Breakfast", "Turkish Coffee", 4m }
                });

            migrationBuilder.UpdateData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "HotelId", "Name", "Price" },
                values: new object[] { "Two grilled koobideh kebabs served with saffron rice, grilled tomato and butter.", 1, "Chelo Kebab Koobideh", 18m });

            migrationBuilder.UpdateData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "HotelId", "Meal", "Name", "Price" },
                values: new object[] { "Grilled marinated beef fillet served with saffron rice and grilled tomato.", 1, "Lunch", "Chelo Kebab Barg", 24m });

            migrationBuilder.UpdateData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "HotelId", "Meal", "Name", "Price" },
                values: new object[] { "Persian herb stew with kidney beans, dried lime and tender beef, served with rice.", 1, "Lunch", "Ghormeh Sabzi", 16m });

            migrationBuilder.InsertData(
                table: "Food",
                columns: new[] { "Id", "Description", "HotelId", "Meal", "Name", "Price" },
                values: new object[,]
                {
                    { 4, "Chicken cooked in a rich walnut and pomegranate sauce, served with saffron rice.", 1, "Dinner", "Fesenjan", 19m },
                    { 5, "Saffron rice with barberries and roasted chicken.", 1, "Dinner", "Zereshk Polo Ba Morgh", 17m },
                    { 6, "Saffron-marinated grilled chicken pieces served with rice and grilled vegetables.", 1, "Dinner", "Joojeh Kebab", 17m },
                    { 7, "Persian yogurt with cucumber, dried mint and herbs.", 1, "Starter", "Mast-o-Khiar", 7m },
                    { 8, "Roasted eggplant dip topped with whey, fried onions, walnuts and mint.", 1, "Starter", "Kashk-e Bademjan", 9m },
                    { 9, "Fresh bread, feta cheese, walnuts, cucumber, tomato, butter and honey.", 1, "Breakfast", "Persian Breakfast", 11m },
                    { 10, "Traditional Persian ice cream flavored with saffron, rose water and pistachios.", 1, "Dessert", "Saffron Ice Cream", 8m },
                    { 11, "Tender grilled lamb chops marinated with saffron and served with rice.", 2, "Lunch", "Shishlik", 27m },
                    { 12, "Grilled kebab served with saffron rice, grilled tomato and fresh herbs.", 2, "Lunch", "Chelo Kebab Mashhadi", 19m },
                    { 13, "Split pea stew with beef, tomato and dried lime served with rice.", 2, "Lunch", "Mashhadi Gheymeh", 15m },
                    { 14, "Persian rice with noodles, raisins and dates served with tender chicken.", 2, "Lunch", "Reshteh Polo", 16m },
                    { 15, "Saffron rice with fava beans and dill served with slow-cooked lamb shank.", 2, "Dinner", "Baghali Polo Ba Mahicheh", 25m },
                    { 16, "Combination of koobideh and barg kebab served with saffron rice.", 2, "Dinner", "Kebab Soltani", 26m },
                    { 17, "Traditional thick soup with beans, herbs, noodles and whey.", 2, "Starter", "Ash Reshteh", 10m },
                    { 18, "Smoked eggplant with tomato, garlic and egg.", 2, "Starter", "Mirza Ghasemi", 9m },
                    { 19, "Eggs, cheese, butter, honey, jam, fresh bread, cucumber and tomato.", 2, "Breakfast", "Hotel Breakfast", 12m },
                    { 20, "Traditional Persian saffron rice pudding with cinnamon and pistachios.", 2, "Dessert", "Sholeh Zard", 7m },
                    { 21, "Traditional Isfahani minced lamb and herbs served with freshly baked bread.", 3, "Lunch", "Beryani", 18m },
                    { 22, "Sweet and creamy Isfahani yogurt dessert made with saffron and orange zest.", 3, "Dessert", "Khoresht Mast", 9m },
                    { 23, "Chicken in walnut and pomegranate sauce served with saffron rice.", 3, "Lunch", "Khoresht Fesenjan", 19m },
                    { 24, "Tender beef and yellow plum stew served with Persian saffron rice.", 3, "Lunch", "Khoresht Alu", 17m },
                    { 25, "Smoked eggplant topped with whey, walnuts, mint and fried onions.", 3, "Starter", "Kashk-e Bademjan", 9m },
                    { 26, "Grape leaves stuffed with rice, herbs and seasoned meat.", 3, "Starter", "Dolmeh Barg", 12m },
                    { 27, "Persian rice with lentils, raisins and dates served with roasted chicken.", 3, "Dinner", "Adas Polo", 16m },
                    { 28, "Saffron-marinated chicken kebab served with rice and grilled vegetables.", 3, "Dinner", "Joojeh Kebab", 17m },
                    { 29, "Fresh bread, local cheese, walnuts, honey, cream, cucumber and tomato.", 3, "Breakfast", "Traditional Isfahani Breakfast", 11m },
                    { 30, "Traditional Isfahani nougat made with pistachios and rose water.", 3, "Dessert", "Gaz", 8m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ReservationId",
                table: "Order",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Food_HotelId",
                table: "Food",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Drink_HotelId",
                table: "Drink",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drink_Hotel_HotelId",
                table: "Drink",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Hotel_HotelId",
                table: "Food",
                column: "HotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Reservation_ReservationId",
                table: "Order",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drink_Hotel_HotelId",
                table: "Drink");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_Hotel_HotelId",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Reservation_ReservationId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ReservationId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Food_HotelId",
                table: "Food");

            migrationBuilder.DropIndex(
                name: "IX_Drink_HotelId",
                table: "Drink");

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Drink");

            migrationBuilder.UpdateData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Traditional Persian tea.", "Tea", 2m });

            migrationBuilder.UpdateData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Fresh orange juice.", "Orange Juice", 4m });

            migrationBuilder.UpdateData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Cold soft drink.", "Cola", 3m });

            migrationBuilder.UpdateData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Traditional grilled kebab.", "Kebab", 15m });

            migrationBuilder.UpdateData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Meal", "Name", "Price" },
                values: new object[] { "Cheese and tomato pizza.", "Dinner", "Pizza", 12m });

            migrationBuilder.UpdateData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Meal", "Name", "Price" },
                values: new object[] { "Traditional Iranian breakfast.", "Breakfast", "Breakfast", 8m });
        }
    }
}
