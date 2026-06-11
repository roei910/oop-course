import { UserStockNoteDeleteRequest } from './user-stock-note-delete-request';

describe('UserStockNoteDeleteRequest', () => {
  it('should have correct shape', () => {
    const req: UserStockNoteDeleteRequest = {
      userEmail: 'test@test.com',
      stockSymbol: 'AAPL',
      noteId: 'n1'
    };

    expect(req.userEmail).toBe('test@test.com');
    expect(req.stockSymbol).toBe('AAPL');
    expect(req.noteId).toBe('n1');
  });
});
