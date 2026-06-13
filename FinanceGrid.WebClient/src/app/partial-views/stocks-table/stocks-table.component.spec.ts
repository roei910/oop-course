import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

import { StocksTableComponent } from './stocks-table.component';

describe('StocksTableComponent', () => {
  let component: StocksTableComponent;
  let fixture: ComponentFixture<StocksTableComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StocksTableComponent],
      providers: [provideHttpClient(), provideHttpClientTesting()],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    });
    fixture = TestBed.createComponent(StocksTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
