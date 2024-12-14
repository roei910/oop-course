import { TrendingStock } from "./trending-stock";

export class StockNews {
    articleTitle?: string;
    articleUrl?: string;
    articlePhotoUrl?: string;
    source?: string;
    postTimeUtc?: string;
    stocksInNews?: TrendingStock[];
}