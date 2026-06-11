export class StockNotification {
    id?: string;
    stockSymbol!: string;
    userEmail!: string;
    targetPrice!: number;
    isTargetBiggerThanOrEqual?: boolean;
    shouldBeNotified?: boolean;
}