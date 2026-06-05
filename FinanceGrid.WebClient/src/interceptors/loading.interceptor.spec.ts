import {
  HttpRequest,
  HttpHandlerFn,
  HttpEvent
} from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { interceptLoader } from './loading.interceptor';
import { LoadingService } from 'src/services/loading.service';
import { TestBed } from '@angular/core/testing';

describe('LoadingInterceptor', () => {
  let loadingService: LoadingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    loadingService = TestBed.inject(LoadingService);
  });

  function createRequest(url: string): HttpRequest<unknown> {
    return new HttpRequest('GET', url);
  }

  it('should show loading on request start', () => {
    const spy = spyOn(loadingService, 'show');
    const req = createRequest('/test');
    const next: HttpHandlerFn = () => of(null as unknown as HttpEvent<unknown>);

    TestBed.runInInjectionContext(() => {
      interceptLoader(req, next).subscribe();
    });

    expect(spy).toHaveBeenCalled();
  });

  it('should hide loading after response completes', () => {
    const showSpy = spyOn(loadingService, 'show');
    const hideSpy = spyOn(loadingService, 'hide');
    const req = createRequest('/test');
    const next: HttpHandlerFn = () => of(null as unknown as HttpEvent<unknown>);

    TestBed.runInInjectionContext(() => {
      interceptLoader(req, next).subscribe({
        complete: () => {
          expect(showSpy).toHaveBeenCalled();
          expect(hideSpy).toHaveBeenCalled();
        }
      });
    });
  });
});
