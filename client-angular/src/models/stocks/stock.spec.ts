import { Stock } from './stock';

describe('Stock', () => {
  it('should have the correct structure', () => {
    const stock: Stock = {
      id: '1',
      name: 'Test',
      symbol: 'TST',
      price: 100,
      regularMarketPreviousClose: 0,
      regularMarketOpen: 0,
      regularMarketDayLow: 0,
      regularMarketDayHigh: 0,
      regularMarketDayRange: '',
      regularMarketChange: 0,
      regularMarketChangePercent: 0,
      regularMarketVolume: 0,
      fiftyDayAverage: 0,
      twoHundredDayAverage: 0,
      fiftyTwoWeekRange: '',
      fiftyTwoWeekLow: 0,
      fiftyTwoWeekHigh: 0,
      targetPriceLow: 0,
      targetPriceHigh: 0,
      targetPriceMean: 0,
      targetPriceMedian: 0,
      forwardPE: 0,
      epsCurrentYear: 0,
      epsForward: 0,
      fullExchangeName: '',
      analystRating: undefined,
      updatedTime: new Date(),
      analysis: undefined
    };
    expect(stock).toBeTruthy();
    expect(stock.symbol).toBe('TST');
  });
});
