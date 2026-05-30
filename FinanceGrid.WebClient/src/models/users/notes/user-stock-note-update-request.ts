export type UserStockNoteUpdateRequest = {
    userEmail: string;
    stockSymbol: string;
    noteId: string;
    updatedNote: string;
}