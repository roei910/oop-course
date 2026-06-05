import { StockHistory } from './stock-history';

describe('StockHistory', () => {
  it('should have correct shape', () => {
    const history: StockHistory = {
      date: '2024-01-15',
      priceOpen: 150,
      priceClose: 152,
      dayLow: 148,
      dayHigh: 153,
      dayRange: '148-153',
      dayVolume: 1000000
    };

    expect(history.date).toBe('2024-01-15');
    expect(history.priceOpen).toBe(150);
    expect(history.dayLow).toBe(148);
    expect(history.dayHigh).toBe(153);
    expect(history.dayVolume).toBe(1000000);
  });
});
