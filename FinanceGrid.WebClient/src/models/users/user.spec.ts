import { User } from './user';
import { StockNotification } from './stock-notification';
import { UserStockNote } from './notes/user-stock-note';

describe('User', () => {
  it('should create an instance', () => {
    expect(new User()).toBeTruthy();
  });

  it('should hold user properties', () => {
    const user = new User();
    user.id = 'u1';
    user.firstName = 'John';
    user.lastName = 'Doe';
    user.email = 'john@test.com';
    user.watchListNames = ['Tech', 'Dividends'];
    user.stockNotifications = [];
    user.userStockNotesBySymbol = {};

    expect(user.email).toBe('john@test.com');
    expect(user.watchListNames.length).toBe(2);
  });

  it('should hold stock notifications', () => {
    const notification: StockNotification = {
      id: 'n1', stockSymbol: 'AAPL',
      userEmail: 'john@test.com', targetPrice: 160
    };
    const user = new User();
    user.stockNotifications = [notification];

    expect(user.stockNotifications.length).toBe(1);
    expect(user.stockNotifications[0].stockSymbol).toBe('AAPL');
  });

  it('should hold notes by symbol', () => {
    const note: UserStockNote = {
      id: 'note1', note: 'Strong buy',
      creationTime: new Date(), lastUpdateTime: new Date()
    };
    const user = new User();
    user.userStockNotesBySymbol = { 'AAPL': [note] };

    expect(user.userStockNotesBySymbol['AAPL'].length).toBe(1);
    expect(user.userStockNotesBySymbol['AAPL'][0].note).toBe('Strong buy');
  });
});
