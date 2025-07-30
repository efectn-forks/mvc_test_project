using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace mvc_proje.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Posts",
                type: "TEXT",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Devices and gadgets", "Electronics" },
                    { 2, "Literature and educational materials", "Books" },
                    { 3, "Apparel and accessories", "Clothing" },
                    { 4, "Household items and kitchenware", "Home & Kitchen" },
                    { 5, "Equipment for sports and outdoor activities", "Sports & Outdoors" }
                });

            migrationBuilder.InsertData(
                table: "ContactMessages",
                columns: new[] { "Id", "Email", "Message", "Name", "Subject" },
                values: new object[,]
                {
                    { 1, "test@test.com", "This is a test message.", "John Doe", "Test Subject" },
                    { 2, "test2@test.com", "This is another test message.", "Jane Smith", "Another Test Subject" }
                });

            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "Description", "Icon", "Link", "Title" },
                values: new object[,]
                {
                    { 1, "Product is waterproof", "", "", "Waterproof" },
                    { 2, "Product supports Bluetooth connectivity", "", "", "Bluetooth" },
                    { 3, "Product is energy efficient", "", "", "Energy Efficient" },
                    { 4, "Product includes smart technology features", "", "", "Smart Technology" },
                    { 5, "Product is made from eco-friendly materials", "", "", "Eco-Friendly" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Email", "FullName", "Password", "PhoneNumber", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "123 Admin St, Admin City", "mail1@mail.com", "Admin User", "$2a$11$gu0rv87tx938Dso20uk/ae6jWYDyRCe42mFKRLfi3pAl6VN0qfUhm", "1234567890", 1, "admin" },
                    { 2, "456 User Ave, User City", "mail2@mail.com", "User One", "$2a$11$gu0rv87tx938Dso20uk/ae6jWYDyRCe42mFKRLfi3pAl6VN0qfUhm", "0987654321", 0, "user1" },
                    { 3, "456 User Ave, User City", "mail3@mail.com", "User Three", "$2a$11$gu0rv87tx938Dso20uk/ae6jWYDyRCe42mFKRLfi3pAl6VN0qfUhm", "0987654321", 0, "user3" },
                    { 4, "456 User Ave, User City", "mail4@mail.com", "User Four", "$2a$11$gu0rv87tx938Dso20uk/ae6jWYDyRCe42mFKRLfi3pAl6VN0qfUhm", "0987654321", 0, "user4" },
                    { 5, "456 User Ave, User City", "mail5@mail.com", "User Five", "$2a$11$gu0rv87tx938Dso20uk/ae6jWYDyRCe42mFKRLfi3pAl6VN0qfUhm", "0987654321", 0, "user5" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "Title", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "Dependency Injection (DI) is a design pattern that allows for the decoupling of classes and their dependencies. In ASP.NET Core, DI is built-in and can be configured in the Startup class.", new DateTime(2025, 7, 29, 20, 8, 13, 334, DateTimeKind.Utc).AddTicks(9773), "", "Understanding Dependency Injection in ASP.NET Core", new DateTime(2025, 7, 29, 20, 8, 13, 334, DateTimeKind.Utc).AddTicks(9773), 1 },
                    { 2, "Entity Framework Core (EF Core) is an open-source, lightweight, extensible, and cross-platform version of Entity Framework. It is used to access databases in .NET applications.", new DateTime(2025, 7, 29, 20, 8, 13, 335, DateTimeKind.Utc).AddTicks(1431), "", "Exploring Entity Framework Core", new DateTime(2025, 7, 29, 20, 8, 13, 335, DateTimeKind.Utc).AddTicks(1431), 1 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Name", "Price", "Stock" },
                values: new object[,]
                {
                    { 1, 5, "Description for Product 1", "Product 1", 19.99m, 100 },
                    { 2, 2, "Description for Product 2", "Product 2", 29.99m, 50 },
                    { 3, 1, "Description for Product 3", "Product 3", 39.99m, 75 },
                    { 4, 1, "Description for Product 4", "Product 4", 49.99m, 20 },
                    { 5, 4, "Description for Product 5", "Product 5", 59.99m, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ContactMessages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ContactMessages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);

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

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

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
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Posts");
        }
    }
}
