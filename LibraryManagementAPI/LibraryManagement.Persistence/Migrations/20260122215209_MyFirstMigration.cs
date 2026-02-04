using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MyFirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patrons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    MembershipDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patrons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ISBN = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    PublicationYear = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BorrowRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    PatronId = table.Column<int>(type: "int", nullable: false),
                    BorrowDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowRecords_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BorrowRecords_Patrons_PatronId",
                        column: x => x.PatronId,
                        principalTable: "Patrons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "English novelist", new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "George", "Orwell" },
                    { 2, "British author", new DateTime(1965, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "J.K.", "Rowling" },
                    { 3, "Famous mystery writer", new DateTime(1890, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agatha", "Christie" },
                    { 4, "American novelist", new DateTime(1896, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "F. Scott", "Fitzgerald" }
                });

            migrationBuilder.InsertData(
                table: "Patrons",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "MembershipDate" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Alice", "Johnson", new DateTime(2025, 1, 22, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2628) },
                    { 2, "bob@example.com", "Bob", "Smith", new DateTime(2025, 7, 22, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2634) },
                    { 3, "charlie@example.com", "Charlie", "Brown", new DateTime(2025, 11, 22, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2639) }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "CoverImageUrl", "Description", "ISBN", "PublicationYear", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, 1, null, "Dystopian novel", "9780451524935", 1949, 5, "1984" },
                    { 2, 1, null, "Political satire", "9780451526342", 1945, 4, "Animal Farm" },
                    { 3, 2, null, "Fantasy novel", "9780439708180", 1997, 10, "Harry Potter and the Sorcerer's Stone" },
                    { 4, 2, null, "Fantasy novel", "9780439064873", 1998, 8, "Harry Potter and the Chamber of Secrets" },
                    { 5, 3, null, "Mystery novel", "9780062693662", 1934, 3, "Murder on the Orient Express" },
                    { 6, 3, null, "Mystery novel", "9780062073488", 1937, 3, "Death on the Nile" },
                    { 7, 4, null, "Classic novel", "9780743273565", 1925, 6, "The Great Gatsby" },
                    { 8, 4, null, "Classic novel", "9780684801544", 1934, 5, "Tender Is the Night" }
                });

            migrationBuilder.InsertData(
                table: "BorrowRecords",
                columns: new[] { "Id", "BookId", "BorrowDate", "DueDate", "PatronId", "ReturnDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 1, 12, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2658), new DateTime(2026, 2, 11, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2661), 1, null, 1 },
                    { 2, 3, new DateTime(2026, 1, 17, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2664), new DateTime(2026, 2, 16, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2666), 2, null, 1 },
                    { 3, 5, new DateTime(2026, 1, 20, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2667), new DateTime(2026, 2, 19, 21, 52, 9, 625, DateTimeKind.Utc).AddTicks(2667), 3, null, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRecords_BookId",
                table: "BorrowRecords",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowRecords_PatronId",
                table: "BorrowRecords",
                column: "PatronId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowRecords");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Patrons");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
