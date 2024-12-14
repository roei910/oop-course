import { StockAnalysis } from "./stock-analysis";

export class Stock {
    id!: string;
    name!: string;
    symbol!: string;
    price!: number;
    fiftyDayAverage!: number;
    twoHundredDayAverage!: number
    fiftyTwoWeekRange!: string;
    fiftyTwoWeekLow!: number;
    fiftyTwoWeekHigh!: number;
    analystRating?: string;
    updatedTime!: Date;
    analysis?: StockAnalysis;
}