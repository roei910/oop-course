import {
  HttpRequest,
  HttpEvent,
  HttpHandlerFn,
  HttpErrorResponse
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, Observable, of, throwError, timeout } from 'rxjs';
import { ToastService } from 'src/services/toast.service';

export function interceptConnection (req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {
  let requestTimeoutMs = 30000;
  let toastService = inject(ToastService);

  return next(req).pipe(
    timeout(requestTimeoutMs),
    catchError((error: HttpErrorResponse) => {
      if(error.status == 0 || error.status >= 500 || error.status == undefined){
        toastService.addErrorMessage(error.message);

        return of();
      }

      return throwError(() => error);
    })
  );
};