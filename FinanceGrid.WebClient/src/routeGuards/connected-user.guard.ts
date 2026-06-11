import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthenticationService } from 'src/services/authentication.service';

export const connectedUserGuard: CanActivateFn = (route, state) => {
  let router = inject(Router);
  let authenticationService = inject(AuthenticationService);

  if(authenticationService.isUserConnected())
    return true;

  router.navigate(['/login']);

  return false;
};