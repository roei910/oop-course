import { TestBed } from '@angular/core/testing';
import { LoadingService } from './loading.service';

describe('LoadingService', () => {
  let service: LoadingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LoadingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should start with loading as false', (done) => {
    service.loading$.subscribe(value => {
      expect(value).toBeFalse();
      done();
    });
  });

  it('should set loading to true on show', (done) => {
    service.show();

    service.loading$.subscribe(value => {
      expect(value).toBeTrue();
      done();
    });
  });

  it('should set loading to false on hide', (done) => {
    service.show();
    service.hide();

    service.loading$.subscribe(value => {
      expect(value).toBeFalse();
      done();
    });
  });
});
