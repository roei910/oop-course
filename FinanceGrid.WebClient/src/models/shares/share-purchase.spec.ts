import { SharePurchase } from './share-purchase';

describe('SharePurchase', () => {
  it('should create an instance', () => {
    expect(new SharePurchase()).toBeTruthy();
  });

  it('should hold purchase details', () => {
    const purchase = new SharePurchase();
    purchase.userEmail = 'test@test.com';
    purchase.stockSymbol = 'AAPL';
    purchase.purchasingPrice = 150;
    purchase.amount = 10;
    purchase.listName = 'Tech';

    expect(purchase.purchasingPrice).toBe(150);
    expect(purchase.amount).toBe(10);
  });

  it('should have optional purchaseDate', () => {
    const purchase = new SharePurchase();
    expect(purchase.purchaseDate).toBeUndefined();

    purchase.purchaseDate = new Date('2024-01-15');
    expect(purchase.purchaseDate).toEqual(new Date('2024-01-15'));
  });
});
