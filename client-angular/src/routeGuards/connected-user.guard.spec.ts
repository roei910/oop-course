import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';
import { connectedUserGuard } from './connected-user.guard';

describe('connectedUserGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) => 
      TestBed.runInInjectionContext(() => connectedUserGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
