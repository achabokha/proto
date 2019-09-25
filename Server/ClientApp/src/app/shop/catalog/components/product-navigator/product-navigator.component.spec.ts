import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductNavigatorComponent } from './product-navigator.component';

describe('ProductNavigatorComponent', () => {
  let component: ProductNavigatorComponent;
  let fixture: ComponentFixture<ProductNavigatorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductNavigatorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductNavigatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
