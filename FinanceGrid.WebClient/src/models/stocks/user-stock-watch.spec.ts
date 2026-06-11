import { UserStockWatch } from './user-stock-watch';
import { Share } from '../shares/share';

describe('UserStockWatch', () => {
  it('should create an instance', () => {
    expect(new UserStockWatch()).toBeTruthy();
  });

  it('should hold shares by purchase guid', () => {
    const share: Share = { id: 's1', purchasingPrice: 150, amount: 10 };
    const watch = new UserStockWatch();
    watch.id = '1';
    watch.userEmail = 'test@test.com';
    watch.listName = 'Tech';
    watch.stockSymbol = 'AAPL';
    watch.purchaseGuidToShares = { 's1': share };

    expect(watch.purchaseGuidToShares['s1']).toBe(share);
    expect(watch.purchaseGuidToShares['s1'].purchasingPrice).toBe(150);
  });

  it('should have an optional note', () => {
    const watch = new UserStockWatch();
    expect(watch.note).toBeUndefined();

    watch.note = 'Looking good';
    expect(watch.note).toBe('Looking good');
  });
});
