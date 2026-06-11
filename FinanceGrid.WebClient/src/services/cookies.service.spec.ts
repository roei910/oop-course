import { TestBed } from '@angular/core/testing';
import { CookiesService } from './cookies.service';

describe('CookiesService', () => {
  let service: CookiesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CookiesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getCookie', () => {
    it('should return cookie value when found', () => {
      Object.defineProperty(document, 'cookie', {
        value: 'email=test@example.com; theme=dark',
        writable: true
      });

      expect(service.getCookie('email')).toBe('test@example.com');
    });

    it('should return null when cookie not found', () => {
      Object.defineProperty(document, 'cookie', {
        value: 'theme=dark',
        writable: true
      });

      expect(service.getCookie('email')).toBeNull();
    });

    it('should return null for empty document.cookie', () => {
      Object.defineProperty(document, 'cookie', {
        value: '',
        writable: true
      });

      expect(service.getCookie('anything')).toBeNull();
    });
  });

  describe('setCookie', () => {
    it('should set a cookie with path=/', () => {
      Object.defineProperty(document, 'cookie', {
        value: '',
        writable: true
      });

      service.setCookie('testKey', 'testValue', 7);

      expect(document.cookie).toContain('testKey=testValue');
      expect(document.cookie).toContain('; path=/');
    });

    it('should set a cookie without expiry when no days given', () => {
      Object.defineProperty(document, 'cookie', {
        value: '',
        writable: true
      });

      service.setCookie('testKey', 'testValue');

      expect(document.cookie).not.toContain('expires=');
      expect(document.cookie).toContain('testKey=testValue');
    });
  });

  describe('deleteCookie', () => {
    it('should set cookie with past expiry', () => {
      Object.defineProperty(document, 'cookie', {
        value: '',
        writable: true
      });

      service.deleteCookie('testKey');

      expect(document.cookie).toContain('testKey=');
      expect(document.cookie).toContain('expires=');
      expect(document.cookie).toContain('; path=/');
    });
  });
});
