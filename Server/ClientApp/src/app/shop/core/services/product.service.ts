
import { map, publishLast, refCount } from 'rxjs/operators';
import { ProductNav } from '../../catalog/models/product-nav';
import { Product } from '../../catalog/models/product';
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable()
export class ProductService {

  list$: Observable<Product[]>;


  constructor(private http: HttpClient) {
    // keep in cache the last result
    this.list$ = this.loadProducts().pipe(publishLast(), refCount());
  }

  fetchProducts(): Observable<Product[]> { // Array<Product>
    return this.list$;
  }


  getProduct(productId: string): Observable<ProductNav> {
    return this.fetchProducts().pipe(map(products => {

      const product = products.filter(b => b.id === productId)[0];
      const index = products.indexOf(product);
      const count = products.length;
      const previousId = index > 0 ? products[index - 1].id : null;
      const nextId = index < count - 1 ? products[index + 1].id : null;
      return { product, previousId, nextId, index, count };
    }));
  }

  loadProducts(): Observable<Product[]> {
    return this.http.get(environment.apiUrl + "/api/products").pipe(map(data => (data as Product[])));

  }


}
