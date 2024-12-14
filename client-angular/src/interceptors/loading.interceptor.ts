import { inject } from '@angular/core';
import {
  HttpRequest,
  HttpEvent,
  HttpHandlerFn
} from '@angular/common/http';
import { finalize, Observable } from 'rxjs';
import { LoadingService } from 'src/services/loading.service';

export function interceptLoader(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>>{
  let loadingService = inject(LoadingService);

  loadingService.show();
  
  return next(req).pipe(
    finalize(() => loadingService.hide())
  );
}