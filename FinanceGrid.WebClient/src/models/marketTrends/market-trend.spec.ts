import { MarketTrend } from './market-trend';
import { TrendingStock } from './trending-stock';
import { StockNews } from './stock-news';

describe('MarketTrend', () => {
  it('should create an instance', () => {
    expect(new MarketTrend()).toBeTruthy();
  });

  it('should have correct shape', () => {
    const trend: MarketTrend = {
      trendName: 'MOST_ACTIVE',
      trendingStocks: [],
      stockNews: []
    };

    expect(trend.trendName).toBe('MOST_ACTIVE');
    expect(Array.isArray(trend.trendingStocks)).toBeTrue();
    expect(Array.isArray(trend.stockNews)).toBeTrue();
  });

  it('should hold trending stocks', () => {
    const stock: TrendingStock = { symbol: 'AAPL', name: 'Apple Inc.', price: 150 };
    const trend: MarketTrend = {
      trendName: 'GAINERS',
      trendingStocks: [stock],
      stockNews: []
    };

    expect(trend.trendingStocks.length).toBe(1);
    expect(trend.trendingStocks[0].symbol).toBe('AAPL');
  });

  it('should hold stock news', () => {
    const news: StockNews = { articleTitle: 'Market Update', source: 'Reuters' };
    const trend: MarketTrend = {
      trendName: 'GAINERS',
      trendingStocks: [],
      stockNews: [news]
    };

    expect(trend.stockNews.length).toBe(1);
    expect(trend.stockNews[0].articleTitle).toBe('Market Update');
  });
});
