import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StockSharesComponent } from './stock-shares.component';

describe('StockSharesComponent', () => {
  let component: StockSharesComponent;
  let fixture: ComponentFixture<StockSharesComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StockSharesComponent]
    });
    fixture = TestBed.createComponent(StockSharesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
