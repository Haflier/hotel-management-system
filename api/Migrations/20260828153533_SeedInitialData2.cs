using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Room_RoomId",
                table: "Service");

            migrationBuilder.DropIndex(
                name: "IX_Service_RoomId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "ReservedDates",
                table: "Room");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Service",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<List<DateTime>>(
                name: "ReservedDates",
                table: "Room",
                type: "timestamp with time zone[]",
                nullable: false);

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 1,
                column: "ReservedDates",
                value: new List<DateTime>());

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 2,
                column: "ReservedDates",
                value: new List<DateTime>());

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 3,
                column: "ReservedDates",
                value: new List<DateTime>());

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 4,
                column: "ReservedDates",
                value: new List<DateTime>());

            migrationBuilder.UpdateData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 5,
                column: "ReservedDates",
                value: new List<DateTime>());

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoomId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoomId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Service",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoomId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Service_RoomId",
                table: "Service",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Room_RoomId",
                table: "Service",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");
        }
    }
}
