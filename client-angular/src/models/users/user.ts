import { StockNotification } from "./stock-notification";
import { UserStockNote } from "./user-stock-note";

export class User {
    id!: string;
    firstName!: string;
    lastName!: string;
    email!: string;
    watchListNames!: string[];
    stockNotifications!: StockNotification[];
    userStockNotesBySymbol!: {
        [stockSymbol: string]: UserStockNote[]
    }
}