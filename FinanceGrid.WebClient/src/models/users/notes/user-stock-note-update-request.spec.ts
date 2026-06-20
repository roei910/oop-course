import { UserStockNoteUpdateRequest } from './user-stock-note-update-request';

describe('UserStockNoteUpdateRequest', () => {
  it('should have correct shape', () => {
    const req: UserStockNoteUpdateRequest = {
      userEmail: 'test@test.com',
      stockSymbol: 'AAPL',
      noteId: 'n1',
      updatedNote: 'Revised analysis'
    };

    expect(req.userEmail).toBe('test@test.com');
    expect(req.noteId).toBe('n1');
    expect(req.updatedNote).toBe('Revised analysis');
  });
});
