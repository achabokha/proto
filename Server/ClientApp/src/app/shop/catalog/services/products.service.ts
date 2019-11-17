import {refCount, publishLast, map} from 'rxjs/operators';
import { ProductNav } from '../models/product-nav';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";



import { Product } from "../models/product";

const url = "https://api.mongolab.com/api/1/databases/sfproducts/collections/sfproducts/";
const apiKey = "?apiKey=d3qvB8ldYFW2KSynHRediqLuBLP8JA8i";


@Injectable()
export class CatalogService {

  list$: Observable<Product[]>;


  constructor(private http: HttpClient) {
    // keep in cache the last result  
    this.list$ = this.http.get(url + apiKey).pipe(map(response => response),publishLast(),refCount(),);

  }

  fetchProducts(): Observable<Product[]> { // Array<Product>
    return this.list$;
  }


  getProduct(productId: string): Observable<ProductNav> {
    // return this.http.get(url+productId+apiKey).map(response => response.json());
    return this.fetchProducts().pipe(map(products => {

      const product = products.filter(b => b.id === productId)[0];
      const index = products.indexOf(product);
      const count = products.length;
      const previousId = index > 0 ? products[index - 1].id : null;
      const nextId = index < count - 1 ? products[index + 1].id : null;
      return { product, previousId, nextId, index, count };
    }));
  }



}