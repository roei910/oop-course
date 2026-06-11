import { TestBed } from '@angular/core/testing';
import { AuthenticationService } from './authentication.service';
import { CookiesService } from './cookies.service';

describe('AuthenticationService', () => {
  let service: AuthenticationService;
  let cookiesService: jasmine.SpyObj<CookiesService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('CookiesService', ['getCookie', 'setCookie', 'deleteCookie']);

    TestBed.configureTestingModule({
      providers: [
        AuthenticationService,
        { provide: CookiesService, useValue: spy }
      ]
    });

    service = TestBed.inject(AuthenticationService);
    cookiesService = TestBed.inject(CookiesService) as jasmine.SpyObj<CookiesService>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getUserEmail', () => {
    it('should return email from cookie', () => {
      cookiesService.getCookie.and.returnValue('test@example.com');
      expect(service.getUserEmail()).toBe('test@example.com');
    });

    it('should return null when no cookie', () => {
      cookiesService.getCookie.and.returnValue(null);
      expect(service.getUserEmail()).toBeNull();
    });
  });

  describe('isUserConnected', () => {
    it('should return true when email cookie exists', () => {
      cookiesService.getCookie.and.returnValue('test@example.com');
      expect(service.isUserConnected()).toBeTrue();
    });

    it('should return false when email cookie is null', () => {
      cookiesService.getCookie.and.returnValue(null);
      expect(service.isUserConnected()).toBeFalse();
    });

    it('should return false when email cookie is empty', () => {
      cookiesService.getCookie.and.returnValue('');
      expect(service.isUserConnected()).toBeFalse();
    });

    it('should update isUserConnectedSubject', () => {
      cookiesService.getCookie.and.returnValue('test@example.com');
      let emittedValue: boolean | undefined;
      service.isUserConnectedSubject.subscribe(v => emittedValue = v);

      service.isUserConnected();

      expect(emittedValue).toBeTrue();
    });
  });

  describe('disconnectUser', () => {
    it('should delete cookie and set connected to false', () => {
      service.disconnectUser();

      expect(cookiesService.deleteCookie).toHaveBeenCalledWith('email');
      expect(service.isUserConnectedSubject.value).toBeFalse();
    });
  });

  describe('updateConnectedUser', () => {
    it('should set cookie and set connected to true', () => {
      service.updateConnectedUser('test@example.com');

      expect(cookiesService.setCookie).toHaveBeenCalledWith('email', 'test@example.com', 1);
      expect(service.isUserConnectedSubject.value).toBeTrue();
    });
  });

  describe('userConnection', () => {
    it('should return observable of connection state', (done) => {
      cookiesService.getCookie.and.returnValue('test@example.com');
      service.isUserConnected();

      service.userConnection().subscribe(state => {
        expect(state).toBeTrue();
        done();
      });
    });

    it('should emit false when user is not connected', (done) => {
      cookiesService.getCookie.and.returnValue(null);
      service.isUserConnected();

      service.userConnection().subscribe(state => {
        expect(state).toBeFalse();
        done();
      });
    });
  });
});
