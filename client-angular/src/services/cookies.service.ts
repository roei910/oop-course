import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CookiesService {

  constructor() { }

  getCookie(name: string): string | null {
    const foundCookies = document.cookie.match('(^|;)\\s*' + name + '\\s*=\\s*([^;]+)');
    const cookie = foundCookies?.pop();

    return cookie ? cookie : null;
  }

  setCookie(name: string, value: string, days?: number): void {
    let expires = '';
    if (days) {
      const date = new Date();
      date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
      expires = '; expires=' + date.toUTCString();
    }
    document.cookie = name + '=' + value + expires + '; path=/';
  }

  deleteCookie(name: string): void {
    this.setCookie(name, '', -1);
  }
}