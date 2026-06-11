import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { StockService } from './stock.service';
import { Stock } from 'src/models/stocks/stock';
import { MarketTrend } from 'src/models/marketTrends/market-trend';
import { environment } from 'src/environments/environment';

describe('StockService', () => {
  let service: StockService;
  let httpMock: HttpTestingController;
  const baseUrl = `${environment.server_url}/Stock`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [StockService]
    });

    service = TestBed.inject(StockService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAllStocks', () => {
    it('should fetch stocks from API on first call', () => {
      const mockStocks: Stock[] = [createMockStock('AAPL', 'Apple')];

      service.getAllStocks().subscribe(stocks => {
        expect(stocks.length).toBe(1);
        expect(stocks[0].symbol).toBe('AAPL');
      });

      const req = httpMock.expectOne(baseUrl);
      expect(req.request.method).toBe('GET');
      req.flush(mockStocks);
    });
  });

  describe('getStockBySymbol', () => {
    it('should fetch a single stock by symbol', () => {
      const mockStock = createMockStock('AAPL', 'Apple');

      service.getStockBySymbol('AAPL').subscribe(stock => {
        expect(stock.symbol).toBe('AAPL');
      });

      const req = httpMock.expectOne(`${baseUrl}/symbol/AAPL`);
      expect(req.request.method).toBe('GET');
      req.flush(mockStock);
    });
  });

  describe('findStocksBySearchTerm', () => {
    it('should search stocks by term', () => {
      const mockStocks: Stock[] = [];

      service.findStocksBySearchTerm('apple').subscribe(stocks => {
        expect(stocks.length).toBe(0);
      });

      const req = httpMock.expectOne(`${baseUrl}/find/apple`);
      expect(req.request.method).toBe('GET');
      req.flush(mockStocks);
    });
  });

  describe('getMarketsTrends', () => {
    it('should fetch market trends', () => {
      const mockTrends: MarketTrend[] = [{
        trendName: 'MOST_ACTIVE',
        trendingStocks: [],
        stockNews: []
      }];

      service.getMarketsTrends().subscribe(trends => {
        expect(trends.length).toBe(1);
        expect(trends[0].trendName).toBe('MOST_ACTIVE');
      });

      const req = httpMock.expectOne(`${baseUrl}/marketTrends`);
      expect(req.request.method).toBe('GET');
      req.flush(mockTrends);
    });
  });

  describe('shouldBeUpdated', () => {
    it('should return true if startTime is undefined', () => {
      expect(service.shouldBeUpdated(undefined, new Date())).toBeTrue();
    });

    it('should return true if less than 30 minutes passed', () => {
      const start = new Date();
      expect(service.shouldBeUpdated(start, start)).toBeTrue();
    });

    it('should return false if more than 30 minutes passed', () => {
      const start = new Date('2024-01-01T00:00:00');
      const end = new Date('2024-01-01T01:00:00');
      expect(service.shouldBeUpdated(start, end)).toBeFalse();
    });
  });
});

function createMockStock(symbol: string, name: string): Stock {
  return {
    id: '1', name, symbol, price: 150,
    regularMarketPreviousClose: 149, regularMarketOpen: 150, regularMarketDayLow: 148,
    regularMarketDayHigh: 152, regularMarketDayRange: '148-152', regularMarketChange: 1,
    regularMarketChangePercent: 0.67, regularMarketVolume: 1000000,
    fiftyDayAverage: 145, twoHundredDayAverage: 140,
    fiftyTwoWeekRange: '120-160', fiftyTwoWeekLow: 120, fiftyTwoWeekHigh: 160,
    targetPriceLow: 140, targetPriceHigh: 170, targetPriceMean: 155, targetPriceMedian: 155,
    forwardPE: 25, epsCurrentYear: 6, epsForward: 7,
    fullExchangeName: 'NASDAQ', analystRating: undefined, updatedTime: new Date(),
    analysis: undefined
  };
}
