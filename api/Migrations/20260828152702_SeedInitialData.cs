using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Tehran" },
                    { 2, "Mashhad" },
                    { 3, "Isfahan" }
                });

            migrationBuilder.InsertData(
                table: "Drink",
                columns: new[] { "Id", "Description", "Meal", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Traditional Persian tea.", "Breakfast", "Tea", 2m },
                    { 2, "Fresh orange juice.", "Breakfast", "Orange Juice", 4m },
                    { 3, "Cold soft drink.", "Lunch", "Cola", 3m }
                });

            migrationBuilder.InsertData(
                table: "Food",
                columns: new[] { "Id", "Description", "Meal", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Traditional grilled kebab.", "Lunch", "Kebab", 15m },
                    { 2, "Cheese and tomato pizza.", "Dinner", "Pizza", 12m },
                    { 3, "Traditional Iranian breakfast.", "Breakfast", "Breakfast", 8m }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "Id", "Description", "Name", "Price", "RoomId" },
                values: new object[,]
                {
                    { 1, "Daily room cleaning.", "Room Cleaning", 10m, null },
                    { 2, "Breakfast delivered to the room.", "Breakfast Service", 15m, null },
                    { 3, "Laundry and ironing service.", "Laundry", 12m, null }
                });

            migrationBuilder.InsertData(
                table: "Hotel",
                columns: new[] { "Id", "Address", "CityId", "Description", "Name", "Phone" },
                values: new object[,]
                {
                    { 1, "Valiasr Street, Tehran", 1, "A luxury hotel in central Tehran.", "Grand Tehran Hotel", "02112345678" },
                    { 2, "Imam Reza Street, Mashhad", 2, "A comfortable hotel near the holy shrine.", "Mashhad Palace Hotel", "05112345678" },
                    { 3, "Naqsh-e Jahan Square, Isfahan", 3, "A traditional hotel in historic Isfahan.", "Isfahan Heritage Hotel", "03112345678" }
                });

            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "Id", "BasePricePerDay", "BedNumbers", "HotelId", "ReservedDates", "RoomNumber" },
                values: new object[,]
                {
                    { 1, 50m, 1, 1, new List<DateTime>(), "101" },
                    { 2, 80m, 2, 1, new List<DateTime>(), "102" },
                    { 3, 70m, 2, 2, new List<DateTime>(), "201" },
                    { 4, 100m, 3, 2, new List<DateTime>(), "202" },
                    { 5, 75m, 2, 3, new List<DateTime>(), "301" }
                });

            migrationBuilder.InsertData(
                table: "RoomService",
                columns: new[] { "RoomId", "ServiceId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 3 },
                    { 3, 1 },
                    { 4, 1 },
                    { 4, 2 },
                    { 5, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Drink",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Food",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoomService",
                keyColumns: new[] { "RoomId", "ServiceId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "RoomService",
                keyColumns: new[] { "RoomId", "ServiceId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "RoomService",
                keyColumns: new[] { "RoomId", "ServiceId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "RoomService",
                keyColumns: new[] { "RoomId", "ServiceId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "RoomService",
                keyColumns: new[] { "RoomId", "ServiceId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "RoomService",
                keyColumns: new[] { "RoomId", "ServiceId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "RoomService",
                keyColumns: new[] { "RoomId", "ServiceId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "RoomService",
                keyColumns: new[] { "RoomId", "ServiceId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Hotel",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotel",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotel",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "City",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
