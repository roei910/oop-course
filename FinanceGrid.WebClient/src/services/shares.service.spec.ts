import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { SharesService } from './shares.service';
import { environment } from 'src/environments/environment';
import { SharePurchase } from 'src/models/shares/share-purchase';
import { ShareSale } from 'src/models/shares/share-sale';
import { StockListDetails } from 'src/models/stocks/stock-list-details';
import { Share } from 'src/models/shares/share';

describe('SharesService', () => {
  let service: SharesService;
  let httpMock: HttpTestingController;
  const baseUrl = `${environment.server_url}/Share`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SharesService, provideHttpClient(), provideHttpClientTesting()]
    });
    service = TestBed.inject(SharesService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('addUserShare', () => {
    it('should add a share and return it on success', () => {
      const purchase: SharePurchase = {
        userEmail: 'test@test.com', stockSymbol: 'AAPL',
        purchasingPrice: 150, amount: 10, listName: 'Tech'
      };
      const mockShare: Share = {
        id: 's1', purchasingPrice: 150, amount: 10, purchaseDate: new Date()
      };

      service.addUserShare(purchase).subscribe(result => {
        expect(result).toEqual(mockShare);
      });

      const req = httpMock.expectOne(r => r.url === baseUrl && r.method === 'POST');
      expect(req.request.body).toEqual(purchase);
      req.flush(mockShare, { status: 200, statusText: 'OK' });
    });

    it('should return null on non-200 response', () => {
      const purchase: SharePurchase = {
        userEmail: 'test@test.com', stockSymbol: 'AAPL',
        purchasingPrice: 150, amount: 10, listName: 'Tech'
      };

      service.addUserShare(purchase).subscribe({
        next: () => fail('expected error for non-200'),
        error: (err) => expect(err.status).toBe(404)
      });

      const req = httpMock.expectOne(r => r.url === baseUrl && r.method === 'POST');
      req.flush(null, { status: 404, statusText: 'Not Found' });
    });
  });

  describe('removeUserShare', () => {
    it('should remove a share and return true on success', () => {
      const sale: ShareSale = {
        userEmail: 'test@test.com', listName: 'Tech',
        stockSymbol: 'AAPL', sharePurchaseGuid: 's1'
      };

      service.removeUserShare(sale).subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(r => r.url === baseUrl && r.method === 'DELETE');
      expect(req.request.body).toEqual(sale);
      req.flush(null, { status: 200, statusText: 'OK' });
    });

    it('should return false on non-200 response', () => {
      const sale: ShareSale = {
        userEmail: 'test@test.com', listName: 'Tech',
        stockSymbol: 'AAPL', sharePurchaseGuid: 's1'
      };

      service.removeUserShare(sale).subscribe({
        next: () => fail('expected error for non-200'),
        error: (err) => expect(err.status).toBe(404)
      });

      const req = httpMock.expectOne(r => r.url === baseUrl && r.method === 'DELETE');
      req.flush(null, { status: 404, statusText: 'Not Found' });
    });
  });

  describe('addWatchingStock', () => {
    it('should add a watching stock', () => {
      service.addWatchingStock('test@test.com', 'Tech', 'AAPL').subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(`${baseUrl}/watching-stock`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual({
        email: 'test@test.com', listName: 'Tech', stockSymbol: 'AAPL'
      });
      req.flush(null, { status: 200, statusText: 'OK' });
    });
  });

  describe('removeWatchingStock', () => {
    it('should remove a watching stock', () => {
      service.removeWatchingStock('test@test.com', 'Tech', 'AAPL').subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(`${baseUrl}/watching-stock`);
      expect(req.request.method).toBe('DELETE');
      expect(req.request.body).toEqual({
        email: 'test@test.com', listName: 'Tech', stockSymbol: 'AAPL'
      });
      req.flush(null, { status: 200, statusText: 'OK' });
    });
  });

  describe('updateWatchingStockNote', () => {
    it('should update a note', () => {
      service.updateWatchingStockNote('test@test.com', 'Tech', 'AAPL', 'my note')
        .subscribe(result => {
          expect(result).toBeTrue();
        });

      const req = httpMock.expectOne(`${baseUrl}/watching-stock-note`);
      expect(req.request.method).toBe('PATCH');
      expect(req.request.body).toEqual({
        email: 'test@test.com', listName: 'Tech', stockSymbol: 'AAPL', note: 'my note'
      });
      req.flush(null, { status: 200, statusText: 'OK' });
    });
  });

  describe('addUserList', () => {
    it('should add a user list', () => {
      const details: StockListDetails = { userEmail: 'test@test.com', listName: 'NewList' };

      service.addUserList(details).subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(`${baseUrl}/list`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(details);
      req.flush(null, { status: 200, statusText: 'OK' });
    });
  });

  describe('removeUserList', () => {
    it('should remove a user list', () => {
      const details: StockListDetails = { userEmail: 'test@test.com', listName: 'OldList' };

      service.removeUserList(details).subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(`${baseUrl}/list`);
      expect(req.request.method).toBe('DELETE');
      expect(req.request.body).toEqual(details);
      req.flush(null, { status: 200, statusText: 'OK' });
    });
  });
});
