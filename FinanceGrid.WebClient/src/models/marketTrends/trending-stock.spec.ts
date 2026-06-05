import { TrendingStock } from './trending-stock';

describe('TrendingStock', () => {
  it('should create an instance', () => {
    expect(new TrendingStock()).toBeTruthy();
  });

  it('should have optional properties', () => {
    const stock = new TrendingStock();

    expect(stock.symbol).toBeUndefined();
    expect(stock.name).toBeUndefined();
    expect(stock.price).toBeUndefined();
  });

  it('should hold all market properties', () => {
    const stock: TrendingStock = {
      symbol: 'AAPL', name: 'Apple Inc.', price: 150.50,
      change: 2.5, changePercent: 1.67, previousClose: 148,
      preOrPostMarket: 151, preOrPostMarketChange: 1,
      preOrPostMarketChangePercent: 0.67,
      lastUpdateUtc: '2024-01-15T14:30:00Z',
      currency: 'USD', exchange: 'NASDAQ',
      exchangeOpen: '09:30', exchangeClose: '16:00',
      timezone: 'America/New_York', countryCode: 'US',
      type: 'EQUITY'
    };

    expect(stock.symbol).toBe('AAPL');
    expect(stock.currency).toBe('USD');
    expect(stock.countryCode).toBe('US');
  });
});
