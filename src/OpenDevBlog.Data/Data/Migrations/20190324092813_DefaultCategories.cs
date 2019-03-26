namespace OpenDevBlog.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class DefaultCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "IsDeleted", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2019, 3, 24, 9, 28, 13, 428, DateTimeKind.Utc).AddTicks(8023), null, false, null, "Programing" },
                    { 2, new DateTime(2019, 3, 24, 9, 28, 13, 428, DateTimeKind.Utc).AddTicks(9772), null, false, null, "Web" },
                    { 3, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(54), null, false, null, "Desktop" },
                    { 4, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(58), null, false, null, "Front-end" },
                    { 5, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(80), null, false, null, "Administration" },
                    { 6, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(80), null, false, null, "Operating System" },
                    { 7, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(84), null, false, null, "Game development" },
                    { 8, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(88), null, false, null, "UX" },
                    { 9, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(97), null, false, null, "Design" },
                    { 10, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(101), null, false, null, "Code Quality" },
                    { 11, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(101), null, false, null, "Software Engineering" },
                    { 12, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(105), null, false, null, "Agile" },
                    { 13, new DateTime(2019, 3, 24, 9, 28, 13, 429, DateTimeKind.Utc).AddTicks(110), null, false, null, "Source control" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 13);
        }
    }
}
