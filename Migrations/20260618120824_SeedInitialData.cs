using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniTrainingCenterCatalog.Mvc.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CourseCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Programming" },
                    { 2, "Design" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Email", "FullName" },
                values: new object[,]
                {
                    { 1, "a@gmail.com", "Nguyen Van A" },
                    { 2, "b@gmail.com", "Tran Thi B" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Capacity", "CourseCategoryId", "CourseCode", "CourseName", "EnrolledStudents", "Fee", "Instructor", "StartDate" },
                values: new object[,]
                {
                    { 1, 30, 1, "PRG001", "C# Fundamentals", 20, 1500000m, "Mr. John", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 25, 1, "WEB001", "ASP.NET Core MVC", 18, 2000000m, "Mr. David", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CourseCategories",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
