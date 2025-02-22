import { StockAnalysis } from "./stock-analysis";
import { StockHistory } from "./stock-history";

export type Stock = {
    id: string;
    name: string;
    symbol: string;
    price: number;
    regularMarketPreviousClose: number;
    regularMarketOpen: number;
    regularMarketDayLow: number;
    regularMarketDayHigh: number;
    regularMarketDayRange: string;
    regularMarketChange: number;
    regularMarketChangePercent: number;
    regularMarketVolume: number;
    fiftyDayAverage: number;
    twoHundredDayAverage: number
    fiftyTwoWeekRange: string;
    fiftyTwoWeekLow: number;
    fiftyTwoWeekHigh: number;
    targetPriceLow: number;
    targetPriceHigh: number;
    targetPriceMean: number;
    targetPriceMedian: number;
    forwardPE: number;
    epsCurrentYear: number;
    epsForward: number;
    fullExchangeName: string;
    analystRating: string | undefined;
    updatedTime: Date;
    analysis: StockAnalysis | undefined;
    history: StockHistory[];
}