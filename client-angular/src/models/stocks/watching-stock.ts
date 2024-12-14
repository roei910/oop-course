import { Share } from "../shares/share";

export class WatchingStock {
    purchaseGuidToShares!: {
        [purchaseGuid: string]: Share
    };
    note!: string;
}
