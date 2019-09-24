import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, Effect, ofType } from '@ngrx/effects';

import { CartService } from './../../core/services/cart.service';

import { of } from 'rxjs';
import { tap, mergeMap, map, catchError } from 'rxjs/operators';

import * as catalogActions from './../actions/catalog.actions';
import { ProductService } from '../../core/services/product.service';

@Injectable()
export class CatalogEffects {
  constructor(
    private actions$: Actions,
    private productService: ProductService,
    private cartService: CartService,
    private router: Router
  ) {}

  /**
   * Load Books effects
   *
   * @memberof CatalogEffects
   */
  @Effect()
  loadBooks = this.actions$.pipe(
    ofType(catalogActions.LOAD_PRODUCTS),
    mergeMap(() => this.productService.fetchBooks()),
    map(products => new catalogActions.LoadBooksSuccess(products)),
    catchError(err =>
      of(new catalogActions.LoadBooksFail({ error: err.message }))
    )
  );

  @Effect()
  getBook = this.actions$.pipe(
    ofType(catalogActions.VIEW_PRODUCT),
    map((action: catalogActions.ViewBook) => action.payload),
    mergeMap(selectedBook => this.productService.getBook(selectedBook)),
    map(product => new catalogActions.ViewBookSuccess(product)),
    catchError(err =>
      of(new catalogActions.ViewBookFail({ error: err.message }))
    )
  );

  @Effect({ dispatch: false })
  addBookToCart = this.actions$.pipe(
    ofType(catalogActions.ADD_PRODUCT_TO_CART),
    map((action: catalogActions.AddToCart) => action.payload),
    mergeMap(product => of(this.cartService.add(product))),
    tap(() => this.router.navigate(['/cart/content']))
  );
}
