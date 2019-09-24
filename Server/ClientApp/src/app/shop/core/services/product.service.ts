
import { map, publishLast, refCount } from 'rxjs/operators';
import { BookNav } from '../../catalog/models/book-nav';
import { Book } from '../../catalog/models/book';
import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { HttpClient } from '@angular/common/http';
import { Product } from '../../catalog/models/product';
import { ProductNav } from '../../catalog/models/product-nav';
import { environment } from 'src/environments/environment';




const url = "https://api.mongolab.com/api/1/databases/sfbooks/collections/sfbooks/";
const apiKey = "?apiKey=d3qvB8ldYFW2KSynHRediqLuBLP8JA8i";


@Injectable()
export class ProductService {

  list$: Observable<Product[]>;


  constructor(private http: HttpClient) {
    // keep in cache the last result
    this.list$ = this.loadBooks().pipe(publishLast(), refCount());
  }

  fetchBooks(): Observable<Product[]> { // Array<Book>
    return this.list$;
  }


  getBook(productId: string): Observable<ProductNav> {
    return this.fetchBooks().pipe(map(products => {

      const product = products.filter(b => b.id === productId)[0];
      const index = products.indexOf(product);
      const count = products.length;
      const previousId = index > 0 ? products[index - 1].id : null;
      const nextId = index < count - 1 ? products[index + 1].id : null;
      return { product, previousId, nextId, index, count };
    }));
  }

  loadBooks(): Observable<Product[]> {
    return this.http.get(environment.apiUrl + "/api/products").pipe(map(data => (data as Product[])));

  }


}
