import { StockNews } from "./stock-news";
import { TrendingStock } from "./trending-stock";

export class MarketTrend {
    trendName!: string;
    trendingStocks!: TrendingStock[];
    stockNews!: StockNews[];
}