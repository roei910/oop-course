import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController
} from '@angular/common/http/testing';
import { WatchesService, WatchesByList } from './watches.service';
import { UserStockWatch } from 'src/models/stocks/user-stock-watch';
import { Share } from 'src/models/shares/share';
import { environment } from 'src/environments/environment';

describe('WatchesService', () => {
  let service: WatchesService;
  let httpMock: HttpTestingController;
  const baseUrl = `${environment.server_url}/UserStockWatch`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [WatchesService]
    });
    service = TestBed.inject(WatchesService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('loadWatches', () => {
    it('should fetch watches and group by list', (done) => {
      const mockWatches: UserStockWatch[] = [
        { id: '1', userEmail: 'test@test.com', listName: 'Tech', stockSymbol: 'AAPL', purchaseGuidToShares: {} },
        { id: '2', userEmail: 'test@test.com', listName: 'Tech', stockSymbol: 'MSFT', purchaseGuidToShares: {} },
        { id: '3', userEmail: 'test@test.com', listName: 'Dividends', stockSymbol: 'KO', purchaseGuidToShares: {} }
      ];

      service.loadWatches('test@test.com');

      const req = httpMock.expectOne(req =>
        req.url === baseUrl && req.params.get('email') === 'test@test.com'
      );
      req.flush(mockWatches);

      service.getWatches().subscribe(grouped => {
        expect(grouped['Tech']).toBeDefined();
        expect(grouped['Dividends']).toBeDefined();
        expect(Object.keys(grouped['Tech']).length).toBe(2);
        done();
      });
    });

    it('should handle empty watch list', (done) => {
      service.loadWatches('test@test.com');

      const req = httpMock.expectOne(req =>
        req.url === baseUrl && req.params.get('email') === 'test@test.com'
      );
      req.flush([]);

      service.getWatches().subscribe(grouped => {
        expect(Object.keys(grouped).length).toBe(0);
        done();
      });
    });
  });

  describe('getWatchesByList', () => {
    it('should return only watches for the given list', (done) => {
      const mockWatches: UserStockWatch[] = [
        { id: '1', userEmail: 't@t.com', listName: 'Tech', stockSymbol: 'AAPL', purchaseGuidToShares: {} },
        { id: '2', userEmail: 't@t.com', listName: 'Dividends', stockSymbol: 'KO', purchaseGuidToShares: {} }
      ];

      service.loadWatches('t@t.com');
      const req = httpMock.expectOne(req => req.url === baseUrl);
      req.flush(mockWatches);

      service.getWatchesByList('Tech').subscribe(grouped => {
        expect(Object.keys(grouped).length).toBe(1);
        expect(grouped['AAPL']).toBeDefined();
        done();
      });
    });

    it('should return empty object for non-existent list', (done) => {
      service.loadWatches('t@t.com');
      const req = httpMock.expectOne(req => req.url === baseUrl);
      req.flush([]);

      service.getWatchesByList('NonExistent').subscribe(grouped => {
        expect(Object.keys(grouped).length).toBe(0);
        done();
      });
    });
  });

  describe('addWatchLocally', () => {
    it('should add watch to a new list', (done) => {
      const watch: UserStockWatch = {
        id: '1', userEmail: 't@t.com', listName: 'NewList',
        stockSymbol: 'AAPL', purchaseGuidToShares: {}
      };

      service.addWatchLocally('NewList', 'AAPL', watch);

      service.getWatches().subscribe(grouped => {
        expect(grouped['NewList']).toBeDefined();
        expect(grouped['NewList']['AAPL']).toBe(watch);
        done();
      });
    });

    it('should add watch to an existing list without deleting others', (done) => {
      const watch1: UserStockWatch = {
        id: '1', userEmail: 't@t.com', listName: 'Tech',
        stockSymbol: 'AAPL', purchaseGuidToShares: {}
      };
      const watch2: UserStockWatch = {
        id: '2', userEmail: 't@t.com', listName: 'Tech',
        stockSymbol: 'MSFT', purchaseGuidToShares: {}
      };

      service.addWatchLocally('Tech', 'AAPL', watch1);
      service.addWatchLocally('Tech', 'MSFT', watch2);

      service.getWatches().subscribe(grouped => {
        expect(Object.keys(grouped['Tech']).length).toBe(2);
        done();
      });
    });
  });

  describe('removeWatchLocally', () => {
    it('should remove a watch from a list', (done) => {
      const watch: UserStockWatch = {
        id: '1', userEmail: 't@t.com', listName: 'Tech',
        stockSymbol: 'AAPL', purchaseGuidToShares: {}
      };

      service.addWatchLocally('Tech', 'AAPL', watch);
      service.removeWatchLocally('Tech', 'AAPL');

      service.getWatchesByList('Tech').subscribe(grouped => {
        expect(Object.keys(grouped).length).toBe(0);
        done();
      });
    });
  });

  describe('addShareLocally', () => {
    it('should add a share to an existing watch', (done) => {
      const watch: UserStockWatch = {
        id: '1', userEmail: 't@t.com', listName: 'Tech',
        stockSymbol: 'AAPL', purchaseGuidToShares: {}
      };
      const share: Share = { id: 's1', purchasingPrice: 150, amount: 10 };

      service.addWatchLocally('Tech', 'AAPL', watch);
      service.addShareLocally('Tech', 'AAPL', 's1', share);

      service.getWatchesByList('Tech').subscribe(grouped => {
        expect(grouped['AAPL'].purchaseGuidToShares['s1']).toBe(share);
        done();
      });
    });
  });

  describe('removeShareLocally', () => {
    it('should remove a share from an existing watch', (done) => {
      const watch: UserStockWatch = {
        id: '1', userEmail: 't@t.com', listName: 'Tech',
        stockSymbol: 'AAPL', purchaseGuidToShares: {}
      };
      const share: Share = { id: 's1', purchasingPrice: 150, amount: 10 };

      service.addWatchLocally('Tech', 'AAPL', watch);
      service.addShareLocally('Tech', 'AAPL', 's1', share);
      service.removeShareLocally('Tech', 'AAPL', 's1');

      service.getWatchesByList('Tech').subscribe(grouped => {
        expect(grouped['AAPL'].purchaseGuidToShares['s1']).toBeUndefined();
        done();
      });
    });
  });

  describe('updateNoteLocally', () => {
    it('should update the note on an existing watch', (done) => {
      const watch: UserStockWatch = {
        id: '1', userEmail: 't@t.com', listName: 'Tech',
        stockSymbol: 'AAPL', note: 'old note', purchaseGuidToShares: {}
      };

      service.addWatchLocally('Tech', 'AAPL', watch);
      service.updateNoteLocally('Tech', 'AAPL', 'new note');

      service.getWatchesByList('Tech').subscribe(grouped => {
        expect(grouped['AAPL'].note).toBe('new note');
        done();
      });
    });
  });
});
