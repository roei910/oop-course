import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StocksTableComponent } from './stocks-table.component';

describe('StocksTableComponent', () => {
  let component: StocksTableComponent;
  let fixture: ComponentFixture<StocksTableComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StocksTableComponent]
    });
    fixture = TestBed.createComponent(StocksTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
