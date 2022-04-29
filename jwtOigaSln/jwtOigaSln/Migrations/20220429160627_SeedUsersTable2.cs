using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace jwtOigaSln.Migrations
{
    public partial class SeedUsersTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "FirstName", "LastName", "UserName" },
                values: new object[,]
                {
                    { 1, "Alejandro", "Moncada", "Alejo0921" },
                    { 2, "Carolina", "Gahona", "CGahona" },
                    { 3, "Santiago", "Castaño", "alejoS" },
                    { 4, "Alejo", "Monsalve", "Monsa13" },
                    { 5, "David", "Alejandro", "Cada123" },
                    { 6, "Alejo1", "absd", "absd" },
                    { 7, "Alejo2", "absd", "absd" },
                    { 8, "absd", "Alejo3", "absd" },
                    { 9, "absd", "absd", "Alejo4" },
                    { 10, "Alejo5", "absd", "absd" },
                    { 11, "absd", "Alejo6", "absd" },
                    { 12, "absd", "absd", "Alejo7" },
                    { 13, "Alejo8", "absd", "absd" },
                    { 14, "absd", "Alejo9", "absd" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
