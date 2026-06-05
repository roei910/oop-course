import { StockNews } from './stock-news';
import { TrendingStock } from './trending-stock';

describe('StockNews', () => {
  it('should create an instance', () => {
    expect(new StockNews()).toBeTruthy();
  });

  it('should have optional properties', () => {
    const news = new StockNews();

    expect(news.articleTitle).toBeUndefined();
    expect(news.articleUrl).toBeUndefined();
    expect(news.source).toBeUndefined();
  });

  it('should hold related stocks', () => {
    const related: TrendingStock = { symbol: 'AAPL', name: 'Apple Inc.' };
    const news: StockNews = {
      articleTitle: 'Apple Reports Earnings',
      articleUrl: 'https://example.com/news/1',
      articlePhotoUrl: 'https://example.com/photo.jpg',
      source: 'Reuters',
      postTimeUtc: '2024-01-15T14:30:00Z',
      stocksInNews: [related]
    };

    expect(news.stocksInNews?.length).toBe(1);
    expect(news.stocksInNews![0].symbol).toBe('AAPL');
  });
});
