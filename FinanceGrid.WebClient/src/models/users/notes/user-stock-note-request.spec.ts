import { UserStockNoteRequest } from './user-stock-note-request';

describe('UserStockNoteRequest', () => {
  it('should create an instance', () => {
    expect(new UserStockNoteRequest()).toBeTruthy();
  });

  it('should hold request properties', () => {
    const req = new UserStockNoteRequest();
    req.userEmail = 'test@test.com';
    req.stockSymbol = 'AAPL';
    req.note = 'Great stock';

    expect(req.userEmail).toBe('test@test.com');
    expect(req.stockSymbol).toBe('AAPL');
    expect(req.note).toBe('Great stock');
  });
});
