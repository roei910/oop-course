import { ShareSale } from './share-sale';

describe('ShareSale', () => {
  it('should create an instance', () => {
    expect(new ShareSale()).toBeTruthy();
  });

  it('should hold sale details', () => {
    const sale = new ShareSale();
    sale.userEmail = 'test@test.com';
    sale.listName = 'Tech';
    sale.stockSymbol = 'AAPL';
    sale.sharePurchaseGuid = 's1';

    expect(sale.sharePurchaseGuid).toBe('s1');
    expect(sale.stockSymbol).toBe('AAPL');
  });
});
