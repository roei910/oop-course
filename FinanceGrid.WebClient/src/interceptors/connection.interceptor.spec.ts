import {
  HttpRequest,
  HttpHandlerFn,
  HttpErrorResponse,
  HttpEvent,
  HttpResponse
} from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { interceptConnection } from './connection.interceptor';
import { ToastService } from 'src/services/toast.service';
import { TestBed } from '@angular/core/testing';

describe('ConnectionInterceptor', () => {
  let toastServiceSpy: jasmine.SpyObj<ToastService>;

  beforeEach(() => {
    toastServiceSpy = jasmine.createSpyObj('ToastService', ['addErrorMessage']);

    TestBed.configureTestingModule({
      providers: [
        { provide: ToastService, useValue: toastServiceSpy }
      ]
    });
  });

  function createRequest(url: string): HttpRequest<unknown> {
    return new HttpRequest('GET', url);
  }

  function createNextHandler(response: Observable<HttpEvent<unknown>>): HttpHandlerFn {
    return (_req: HttpRequest<unknown>) => response;
  }

  it('should pass through successful requests', (done) => {
    const req = createRequest('/test');
    const mockResponse = new HttpResponse({ body: { data: 'ok' }, status: 200 });
    const next = createNextHandler(of(mockResponse));

    TestBed.runInInjectionContext(() => {
      interceptConnection(req, next).subscribe({
        next: (event) => {
          expect((event as HttpResponse<unknown>).body).toEqual({ data: 'ok' });
          done();
        }
      });
    });
  });

  it('should show error toast and swallow status 0 errors', (done) => {
    const req = createRequest('/test');
    const next = createNextHandler(throwError(() => new HttpErrorResponse({ status: 0 })));

    TestBed.runInInjectionContext(() => {
      interceptConnection(req, next).subscribe({
        complete: () => {
          expect(toastServiceSpy.addErrorMessage).toHaveBeenCalled();
          done();
        }
      });
    });
  });

  it('should show error toast and swallow 500 errors', (done) => {
    const req = createRequest('/test');
    const next = createNextHandler(throwError(() => new HttpErrorResponse({ status: 500 })));

    TestBed.runInInjectionContext(() => {
      interceptConnection(req, next).subscribe({
        complete: () => {
          expect(toastServiceSpy.addErrorMessage).toHaveBeenCalled();
          done();
        }
      });
    });
  });

  it('should show error toast for undefined status errors', (done) => {
    const req = createRequest('/test');
    const next = createNextHandler(throwError(() => new HttpErrorResponse({ status: undefined as unknown as number })));

    TestBed.runInInjectionContext(() => {
      interceptConnection(req, next).subscribe({
        complete: () => {
          expect(toastServiceSpy.addErrorMessage).toHaveBeenCalled();
          done();
        }
      });
    });
  });

  it('should rethrow 4xx errors', (done) => {
    const req = createRequest('/test');
    const error = new HttpErrorResponse({ status: 404 });
    const next = createNextHandler(throwError(() => error));

    TestBed.runInInjectionContext(() => {
      interceptConnection(req, next).subscribe({
        error: (err) => {
          expect(err.status).toBe(404);
          expect(toastServiceSpy.addErrorMessage).not.toHaveBeenCalled();
          done();
        }
      });
    });
  });

  it('should rethrow 403 errors', (done) => {
    const req = createRequest('/test');
    const error = new HttpErrorResponse({ status: 403 });
    const next = createNextHandler(throwError(() => error));

    TestBed.runInInjectionContext(() => {
      interceptConnection(req, next).subscribe({
        error: (err) => {
          expect(err.status).toBe(403);
          done();
        }
      });
    });
  });
});
