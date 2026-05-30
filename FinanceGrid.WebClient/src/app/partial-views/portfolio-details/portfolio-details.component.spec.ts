import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PortfolioDetailsComponent } from './portfolio-details.component';

describe('PortfolioDetailsComponent', () => {
  let component: PortfolioDetailsComponent;
  let fixture: ComponentFixture<PortfolioDetailsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PortfolioDetailsComponent]
    });
    fixture = TestBed.createComponent(PortfolioDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
