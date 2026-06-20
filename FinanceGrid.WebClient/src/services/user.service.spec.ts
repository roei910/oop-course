import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { UserService } from './user.service';
import { AuthenticationService } from './authentication.service';
import { WatchesService } from './watches.service';
import { environment } from 'src/environments/environment';
import { User } from 'src/models/users/user';
import { UserCreation } from 'src/models/users/user-creation';
import { sha256 } from 'js-sha256';

describe('UserService', () => {
  let service: UserService;
  let httpMock: HttpTestingController;
  let authServiceSpy: jasmine.SpyObj<AuthenticationService>;
  let watchesServiceSpy: jasmine.SpyObj<WatchesService>;
  const baseUrl = `${environment.server_url}/User`;

  beforeEach(() => {
    authServiceSpy = jasmine.createSpyObj('AuthenticationService', [
      'isUserConnected', 'getUserEmail', 'updateConnectedUser'
    ]);
    watchesServiceSpy = jasmine.createSpyObj('WatchesService', ['loadWatches']);

    TestBed.configureTestingModule({
      providers: [
        UserService,
        { provide: AuthenticationService, useValue: authServiceSpy },
        { provide: WatchesService, useValue: watchesServiceSpy },
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(UserService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('createUser', () => {
    it('should hash password and register user', () => {
      const user: UserCreation = {
        firstName: 'John', lastName: 'Doe',
        email: 'john@test.com', password: 'rawpassword'
      };

      service.createUser(user).subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(`${baseUrl}/register`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body.password).toBe(sha256('rawpassword'));
      expect(req.request.body.email).toBe('john@test.com');
      req.flush('', { status: 200, statusText: 'OK' });
    });

    it('should return false on failure', () => {
      const user: UserCreation = {
        firstName: 'John', lastName: 'Doe',
        email: 'john@test.com', password: 'rawpassword'
      };

      service.createUser(user).subscribe({
        next: () => fail('expected error for 400'),
        error: (err) => expect(err.status).toBe(400)
      });

      const req = httpMock.expectOne(`${baseUrl}/register`);
      req.flush('error', { status: 400, statusText: 'Bad Request' });
    });
  });

  describe('tryConnect', () => {
    it('should connect and update auth service on success', () => {
      authServiceSpy.getUserEmail.and.returnValue('test@test.com');

      service.tryConnect('test@test.com', 'password').subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(`${baseUrl}/connect-user`);
      expect(req.request.body.password).toBe(sha256('password'));
      req.flush(null, { status: 200, statusText: 'OK' });

      expect(authServiceSpy.updateConnectedUser).toHaveBeenCalledWith('test@test.com');
    });

    it('should return false on bad credentials', () => {
      service.tryConnect('test@test.com', 'wrong').subscribe({
        next: () => fail('expected error for 401'),
        error: (err) => {
          expect(err.status).toBe(401);
          expect(authServiceSpy.updateConnectedUser).not.toHaveBeenCalled();
        }
      });

      const req = httpMock.expectOne(`${baseUrl}/connect-user`);
      req.flush(null, { status: 401, statusText: 'Unauthorized' });
      expect(authServiceSpy.updateConnectedUser).not.toHaveBeenCalled();
    });
  });

  describe('updatePassword', () => {
    it('should hash new password and update', () => {
      service.updatePassword('test@test.com', 'newpass').subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(`${baseUrl}/update-password`);
      expect(req.request.body.password).toBe(sha256('newpass'));
      expect(req.request.body.email).toBe('test@test.com');
      req.flush('', { status: 200, statusText: 'OK' });
    });
  });

  describe('addNote', () => {
    it('should add a stock note', () => {
      const mockNote = { id: 'n1', note: 'test note', creationTime: new Date(), lastUpdateTime: new Date() };

      service.addNote('test@test.com', 'AAPL', 'test note').subscribe(note => {
        expect(note).toEqual(mockNote);
      });

      const req = httpMock.expectOne(`${baseUrl}/stockNote`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual({
        userEmail: 'test@test.com', stockSymbol: 'AAPL', note: 'test note'
      });
      req.flush(mockNote);
    });
  });

  describe('updateNote', () => {
    it('should update a stock note', () => {
      service.updateNote('test@test.com', 'AAPL', 'n1', 'updated').subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(`${baseUrl}/stockNote`);
      expect(req.request.method).toBe('PATCH');
      expect(req.request.body).toEqual({
        userEmail: 'test@test.com', stockSymbol: 'AAPL', noteId: 'n1', updatedNote: 'updated'
      });
      req.flush(null, { status: 200, statusText: 'OK' });
    });
  });

  describe('deleteNote', () => {
    it('should delete a stock note', () => {
      service.deleteNote('test@test.com', 'AAPL', 'n1').subscribe(result => {
        expect(result).toBeTrue();
      });

      const req = httpMock.expectOne(r =>
        r.url === `${baseUrl}/stockNote` && r.params.get('userEmail') === 'test@test.com'
      );
      expect(req.request.method).toBe('DELETE');
      req.flush(null, { status: 200, statusText: 'OK' });
    });
  });

  describe('getUser', () => {
    it('should fetch user via http and update watches', (done) => {
      authServiceSpy.isUserConnected.and.returnValue(true);
      authServiceSpy.getUserEmail.and.returnValue('test@test.com');

      const mockUser: User = {
        id: 'u1', firstName: 'John', lastName: 'Doe',
        email: 'test@test.com', watchListNames: [],
        stockNotifications: [], userStockNotesBySymbol: {}
      };

      service.getUser().subscribe(user => {
        expect(user.email).toBe('test@test.com');
        done();
      });

      const req = httpMock.expectOne(r =>
        r.url === baseUrl && r.params.get('email') === 'test@test.com'
      );
      req.flush(mockUser);

      expect(watchesServiceSpy.loadWatches).toHaveBeenCalledWith('test@test.com');
    });
  });
});
