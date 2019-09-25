import { TestBed, inject } from '@angular/core/testing';

import { CatalogService } from './products.service';

describe('ProductsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CatalogService]
    });
  });

  it('should be created', inject([CatalogService], (service: CatalogService) => {
    expect(service).toBeTruthy();
  }));
});
