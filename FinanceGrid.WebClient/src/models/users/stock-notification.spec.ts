import { StockNotification } from './stock-notification';

describe('StockNotification', () => {
  it('should create an instance', () => {
    expect(new StockNotification()).toBeTruthy();
  });

  it('should hold notification properties', () => {
    const notification = new StockNotification();
    notification.id = 'n1';
    notification.stockSymbol = 'AAPL';
    notification.userEmail = 'test@test.com';
    notification.targetPrice = 160;
    notification.isTargetBiggerThanOrEqual = true;
    notification.shouldBeNotified = false;

    expect(notification.targetPrice).toBe(160);
    expect(notification.isTargetBiggerThanOrEqual).toBeTrue();
  });

  it('should have optional fields default to undefined', () => {
    const notification = new StockNotification();
    expect(notification.id).toBeUndefined();
    expect(notification.isTargetBiggerThanOrEqual).toBeUndefined();
    expect(notification.shouldBeNotified).toBeUndefined();
  });
});
