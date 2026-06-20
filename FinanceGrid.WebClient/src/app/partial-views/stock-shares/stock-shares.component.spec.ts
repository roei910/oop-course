import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ActivatedRoute, convertToParamMap } from '@angular/router';
import { of } from 'rxjs';
import { MessageService, ConfirmationService } from 'primeng/api';
import { NO_ERRORS_SCHEMA } from '@angular/core';

import { StockSharesComponent } from './stock-shares.component';
import { AuthenticationService } from 'src/services/authentication.service';
import { SharesService } from 'src/services/shares.service';
import { WatchesService } from 'src/services/watches.service';

describe('StockSharesComponent', () => {
  let component: StockSharesComponent;
  let fixture: ComponentFixture<StockSharesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StockSharesComponent],
      providers: [
        { provide: ActivatedRoute, useValue: { paramMap: of(convertToParamMap({})), queryParams: of({}) } },
        { provide: MessageService, useValue: jasmine.createSpyObj('MessageService', ['add']) },
        { provide: ConfirmationService, useValue: jasmine.createSpyObj('ConfirmationService', ['confirm']) },
        { provide: AuthenticationService, useValue: jasmine.createSpyObj('AuthenticationService', ['isUserConnected', 'getUserEmail']) },
        { provide: SharesService, useValue: jasmine.createSpyObj('SharesService', ['getSharesByPurchaseGuid']) },
        { provide: WatchesService, useValue: jasmine.createSpyObj('WatchesService', ['getWatchesByList']) },
        provideHttpClient(),
        provideHttpClientTesting()
      ],
      schemas: [NO_ERRORS_SCHEMA]
    });
    fixture = TestBed.createComponent(StockSharesComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
