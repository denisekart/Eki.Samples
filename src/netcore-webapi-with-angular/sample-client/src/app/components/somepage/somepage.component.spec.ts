import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SomepageComponent } from './somepage.component';

describe('SomepageComponent', () => {
  let component: SomepageComponent;
  let fixture: ComponentFixture<SomepageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SomepageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SomepageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
