using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksApi.Providers.UsersSqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Email", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "StockNotifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    StockSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    TargetPrice = table.Column<double>(type: "REAL", nullable: false),
                    IsTargetBiggerThanOrEqual = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShouldBeNotified = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockNotifications_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStockNotes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StockSymbol = table.Column<string>(type: "TEXT", nullable: true),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStockNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStockNotes_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "UserStockWatches",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    ListName = table.Column<string>(type: "TEXT", nullable: false),
                    StockSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStockWatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStockWatches_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchListNames",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchListNames_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PurchasingPrice = table.Column<double>(type: "REAL", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false),
                    PurchaseGuid = table.Column<string>(type: "TEXT", nullable: true),
                    UserStockWatchId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shares_UserStockWatches_UserStockWatchId",
                        column: x => x.UserStockWatchId,
                        principalTable: "UserStockWatches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shares_PurchaseGuid",
                table: "Shares",
                column: "PurchaseGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shares_UserStockWatchId",
                table: "Shares",
                column: "UserStockWatchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockNotifications_UserEmail",
                table: "StockNotifications",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStockNotes_UserEmail_StockSymbol",
                table: "UserStockNotes",
                columns: new[] { "UserEmail", "StockSymbol" });

            migrationBuilder.CreateIndex(
                name: "IX_UserStockWatches_UserEmail",
                table: "UserStockWatches",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockWatches_UserEmail_ListName",
                table: "UserStockWatches",
                columns: new[] { "UserEmail", "ListName" });

            migrationBuilder.CreateIndex(
                name: "IX_WatchListNames_UserEmail_Name",
                table: "WatchListNames",
                columns: new[] { "UserEmail", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropTable(
                name: "StockNotifications");

            migrationBuilder.DropTable(
                name: "UserStockNotes");

            migrationBuilder.DropTable(
                name: "WatchListNames");

            migrationBuilder.DropTable(
                name: "UserStockWatches");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
