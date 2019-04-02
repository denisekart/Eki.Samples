import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubsubpageComponent } from './subsubpage.component';

describe('SubsubpageComponent', () => {
  let component: SubsubpageComponent;
  let fixture: ComponentFixture<SubsubpageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubsubpageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubsubpageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
