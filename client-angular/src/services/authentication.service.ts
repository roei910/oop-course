import { Injectable } from '@angular/core';
import { CookiesService } from './cookies.service';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  isUserConnectedSubject: BehaviorSubject<boolean> = new BehaviorSubject(false);

  constructor(private cookieService: CookiesService
  ) { }

  getUserEmail(): string | null {
    let email = this.cookieService.getCookie("email");

    return email;
  }

  disconnectUser(): void {
    this.cookieService.deleteCookie("email");
    this.isUserConnectedSubject.next(false);
  }

  userConnection(): Observable<boolean>{
    this.isUserConnected();

    return this.isUserConnectedSubject.asObservable();
  }
  
  isUserConnected(): boolean {
    let user = this.cookieService.getCookie("email");
    let isUserConnected = user != null && user != "";

    this.isUserConnectedSubject.next(isUserConnected);

    return isUserConnected;
  }

  updateConnectedUser(email: string): void{
    this.cookieService.setCookie("email", email, 1);
    this.isUserConnectedSubject.next(true);
  }
}