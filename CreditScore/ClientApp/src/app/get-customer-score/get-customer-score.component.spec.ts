import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GetCustomerScoreComponent } from './get-customer-score.component';

describe('GetCustomerScoreComponent', () => {
  let component: GetCustomerScoreComponent;
  let fixture: ComponentFixture<GetCustomerScoreComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GetCustomerScoreComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GetCustomerScoreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
