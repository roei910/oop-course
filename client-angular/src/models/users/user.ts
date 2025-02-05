import { WatchingStock } from "../stocks/watching-stock";
import { StockNotification } from "./stock-notification";
import { UserStockNote } from "./user-stock-note";

export class User {
    id!: string;
    firstName!: string;
    lastName!: string;
    email!: string;
    watchingStocksByListName!: {
        [listName: string] : {
            [stockSymbol: string]: WatchingStock
        }
    };
    stockNotifications!: StockNotification[];
    userStockNotesBySymbol!: {
        [stockSymbol: string]: UserStockNote[]
    }
}