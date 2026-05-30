using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksApi.Providers.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketTrends",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    TrendName = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdatedTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketTrends", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SearchResults",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    SearchTerm = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    StockSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PriceOpen = table.Column<double>(type: "REAL", nullable: false),
                    PriceClose = table.Column<double>(type: "REAL", nullable: false),
                    DayLow = table.Column<double>(type: "REAL", nullable: false),
                    DayHigh = table.Column<double>(type: "REAL", nullable: false),
                    DayRange = table.Column<string>(type: "TEXT", nullable: false),
                    DayVolume = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Price = table.Column<double>(type: "REAL", nullable: false),
                    RegularMarketPreviousClose = table.Column<double>(type: "REAL", nullable: false),
                    RegularMarketOpen = table.Column<double>(type: "REAL", nullable: false),
                    RegularMarketDayLow = table.Column<double>(type: "REAL", nullable: false),
                    RegularMarketDayHigh = table.Column<double>(type: "REAL", nullable: false),
                    RegularMarketDayRange = table.Column<string>(type: "TEXT", nullable: true),
                    RegularMarketChange = table.Column<double>(type: "REAL", nullable: false),
                    RegularMarketChangePercent = table.Column<double>(type: "REAL", nullable: false),
                    RegularMarketVolume = table.Column<long>(type: "INTEGER", nullable: false),
                    FiftyDayAverage = table.Column<double>(type: "REAL", nullable: false),
                    TwoHundredDayAverage = table.Column<double>(type: "REAL", nullable: false),
                    FiftyTwoWeekRange = table.Column<string>(type: "TEXT", nullable: true),
                    FiftyTwoWeekLow = table.Column<double>(type: "REAL", nullable: false),
                    FiftyTwoWeekHigh = table.Column<double>(type: "REAL", nullable: false),
                    TargetPriceLow = table.Column<double>(type: "REAL", nullable: false),
                    TargetPriceHigh = table.Column<double>(type: "REAL", nullable: false),
                    TargetPriceMean = table.Column<double>(type: "REAL", nullable: false),
                    TargetPriceMedian = table.Column<double>(type: "REAL", nullable: false),
                    ForwardPE = table.Column<double>(type: "REAL", nullable: false),
                    EpsCurrentYear = table.Column<double>(type: "REAL", nullable: false),
                    EpsForward = table.Column<double>(type: "REAL", nullable: false),
                    FullExchangeName = table.Column<string>(type: "TEXT", nullable: true),
                    AnalystRating = table.Column<string>(type: "TEXT", nullable: true),
                    AnalysisSymbol = table.Column<string>(type: "TEXT", nullable: true),
                    AnalysisUpdatedTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AnalysisTargetHighPrice = table.Column<double>(type: "REAL", nullable: true),
                    AnalysisTargetLowPrice = table.Column<double>(type: "REAL", nullable: true),
                    AnalysisTargetMeanPrice = table.Column<double>(type: "REAL", nullable: true),
                    AnalysisTargetMedianPrice = table.Column<double>(type: "REAL", nullable: true),
                    UpdatedTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastHistoryUpdateDate = table.Column<DateOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArticleTitle = table.Column<string>(type: "TEXT", nullable: true),
                    ArticleUrl = table.Column<string>(type: "TEXT", nullable: true),
                    ArticlePhotoUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", nullable: true),
                    PostTimeUtc = table.Column<string>(type: "TEXT", nullable: true),
                    MarketTrendId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockNews_MarketTrends_MarketTrendId",
                        column: x => x.MarketTrendId,
                        principalTable: "MarketTrends",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockTrends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<double>(type: "REAL", nullable: true),
                    Change = table.Column<double>(type: "REAL", nullable: true),
                    ChangePercent = table.Column<double>(type: "REAL", nullable: true),
                    PreviousClose = table.Column<double>(type: "REAL", nullable: true),
                    PreOrPostMarket = table.Column<double>(type: "REAL", nullable: true),
                    PreOrPostMarketChange = table.Column<double>(type: "REAL", nullable: true),
                    PreOrPostMarketChangePercent = table.Column<double>(type: "REAL", nullable: true),
                    LastUpdateUtc = table.Column<string>(type: "TEXT", nullable: true),
                    Currency = table.Column<string>(type: "TEXT", nullable: true),
                    Exchange = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangeOpen = table.Column<string>(type: "TEXT", nullable: true),
                    ExchangeClose = table.Column<string>(type: "TEXT", nullable: true),
                    Timezone = table.Column<string>(type: "TEXT", nullable: true),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: true),
                    MarketTrendId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTrends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTrends_MarketTrends_MarketTrendId",
                        column: x => x.MarketTrendId,
                        principalTable: "MarketTrends",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockSearchResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ExchDisp = table.Column<string>(type: "TEXT", nullable: false),
                    TypeDisp = table.Column<string>(type: "TEXT", nullable: false),
                    SearchResultId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockSearchResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockSearchResults_SearchResults_SearchResultId",
                        column: x => x.SearchResultId,
                        principalTable: "SearchResults",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockNotification",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    StockSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    UserEmail = table.Column<string>(type: "TEXT", nullable: false),
                    TargetPrice = table.Column<double>(type: "REAL", nullable: false),
                    IsTargetBiggerThanOrEqual = table.Column<bool>(type: "INTEGER", nullable: false),
                    ShouldBeNotified = table.Column<bool>(type: "INTEGER", nullable: false),
                    StockId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockNotification_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockNewsStockTrend",
                columns: table => new
                {
                    StockNewsId = table.Column<int>(type: "INTEGER", nullable: false),
                    StockTrendId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockNewsStockTrend", x => new { x.StockNewsId, x.StockTrendId });
                    table.ForeignKey(
                        name: "FK_StockNewsStockTrend_StockNews_StockNewsId",
                        column: x => x.StockNewsId,
                        principalTable: "StockNews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockNewsStockTrend_StockTrends_StockTrendId",
                        column: x => x.StockTrendId,
                        principalTable: "StockTrends",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketTrends_TrendName",
                table: "MarketTrends",
                column: "TrendName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SearchResults_SearchTerm",
                table: "SearchResults",
                column: "SearchTerm",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockHistory_StockSymbol",
                table: "StockHistory",
                column: "StockSymbol");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistory_StockSymbol_Date",
                table: "StockHistory",
                columns: new[] { "StockSymbol", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_StockNews_MarketTrendId",
                table: "StockNews",
                column: "MarketTrendId");

            migrationBuilder.CreateIndex(
                name: "IX_StockNewsStockTrend_StockTrendId",
                table: "StockNewsStockTrend",
                column: "StockTrendId");

            migrationBuilder.CreateIndex(
                name: "IX_StockNotification_StockId",
                table: "StockNotification",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Symbol",
                table: "Stocks",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockSearchResults_SearchResultId",
                table: "StockSearchResults",
                column: "SearchResultId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTrends_MarketTrendId",
                table: "StockTrends",
                column: "MarketTrendId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockHistory");

            migrationBuilder.DropTable(
                name: "StockNewsStockTrend");

            migrationBuilder.DropTable(
                name: "StockNotification");

            migrationBuilder.DropTable(
                name: "StockSearchResults");

            migrationBuilder.DropTable(
                name: "StockNews");

            migrationBuilder.DropTable(
                name: "StockTrends");

            migrationBuilder.DropTable(
                name: "Stocks");

            migrationBuilder.DropTable(
                name: "SearchResults");

            migrationBuilder.DropTable(
                name: "MarketTrends");
        }
    }
}
