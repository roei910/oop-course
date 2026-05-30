import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { sha256 } from 'js-sha256';
import { Observable, Subject, map, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ObjectIdResponse } from 'src/models/object-id-response';
import { StockNotification } from 'src/models/users/stock-notification';
import { User } from 'src/models/users/user';
import { UserCreation } from 'src/models/users/user-creation';
import { AuthenticationService } from './authentication.service';
import { PasswordUpdateRequest } from 'src/models/users/password-update-request';
import { UserStockNoteRequest } from 'src/models/users/notes/user-stock-note-request';
import { UserStockNote } from 'src/models/users/notes/user-stock-note';
import { UserStockNoteDeleteRequest } from 'src/models/users/notes/user-stock-note-delete-request';
import { UserStockNoteUpdateRequest } from 'src/models/users/notes/user-stock-note-update-request';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userEndPointUrl: string = `${environment.server_url}/User`;
  private userSubject: Subject<User> | undefined;

  constructor(
    private httpClient: HttpClient, 
    private authenticationService: AuthenticationService
  ) { }

  getUser(): Observable<User>{
    if(this.userSubject == undefined)
      this.userSubject = new Subject<User>();

    this.updateUser();

    return this.userSubject.asObservable();
  }

  createUser(user: UserCreation): Observable<boolean> {
    user.password = sha256(user.password);
    
    let res = this.httpClient
    .post(`${this.userEndPointUrl}/register`, user,
      {
        observe: 'response',
        responseType: 'text'
      }
    )
    .pipe(map(response => response.status == 200));

    return res;
  }

  addStockNotification(stockNotification: StockNotification): Observable<ObjectIdResponse>{
    let res = this.httpClient
      .post<ObjectIdResponse>(`${this.userEndPointUrl}/notification`, 
        stockNotification);

    return res;
  }

  removeStockNotification(email: string, notificationId: string): Observable<boolean> {
    let res = this.httpClient
      .delete<ObjectIdResponse>(
        `${this.userEndPointUrl}/notification`,
        {
          observe: 'response',
          params: {
            email,
            notificationId
          }
        })
      .pipe(map(response => response.status == 200));

    return res;
  }

  tryConnect(email: string, password: string): Observable<boolean> {
    let passwordHash = sha256(password);

    return this.httpClient
      .post<boolean>(`${this.userEndPointUrl}/connect-user`,
        {
          email: email,
          password: passwordHash
        },
        {
          observe: 'response'
        }
      )
      .pipe(
        map(response => response.status == 200),
        tap(res => {
          if (res) {
            this.authenticationService.updateConnectedUser(email);
          }
        })
      );
  }

  updatePassword(email: string, password: string): Observable<boolean> {
    var passwordUpdateRequest: PasswordUpdateRequest = {
      email,
      password: sha256(password)
    };

    let res = this.httpClient
    .post(`${this.userEndPointUrl}/update-password`, passwordUpdateRequest,
      {
        observe: 'response',
        responseType: 'text'
      }
    )
    .pipe(map(response => response.status == 200));

    return res;
  }

  addNote(email: string, stockSymbol: string, note: string): Observable<UserStockNote> {
    let noteRequest: UserStockNoteRequest = {
      userEmail: email,
      stockSymbol,
      note
    };

    return this.httpClient
      .post<UserStockNote>(`${this.userEndPointUrl}/stockNote`,
        noteRequest
      );
  }

  updateNote(email: string, stockSymbol: string, noteId: string, note: string): Observable<boolean> {
    let noteUpdateRequest: UserStockNoteUpdateRequest = {
      userEmail: email,
      stockSymbol,
      noteId,
      updatedNote: note
    };

    return this.httpClient
      .patch(`${this.userEndPointUrl}/stockNote`,
        noteUpdateRequest, {
          observe: 'response'
        }
      )
      .pipe(map(response => response.status == 200));
  }

  deleteNote(email: string, stockSymbol: string, noteId: string): Observable<boolean> {
    let noteDeleteRequest: UserStockNoteDeleteRequest = {
      userEmail: email,
      stockSymbol,
      noteId
    };

    return this.httpClient
      .delete(`${this.userEndPointUrl}/stockNote`, 
        {
          observe: "response",
          params: noteDeleteRequest
        }
      )
      .pipe(map(response => response.status == 200));
  }

  private updateUser(): void{
    if(!this.authenticationService.isUserConnected())
      return;
    
    let email = this.authenticationService.getUserEmail()!;

    this.httpClient
      .get<User>(this.userEndPointUrl,
        {
          params: {
            email
          }
        }
      )
      .pipe(tap(res => this.userSubject?.next(res)))
      .subscribe(user => user);
  }
}