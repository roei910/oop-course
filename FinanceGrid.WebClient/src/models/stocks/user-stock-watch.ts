import { Share } from "../shares/share";

export class UserStockWatch {
    id!: string;
    userEmail!: string;
    listName!: string;
    stockSymbol!: string;
    note?: string;
    purchaseGuidToShares!: { [purchaseGuid: string]: Share };
}
