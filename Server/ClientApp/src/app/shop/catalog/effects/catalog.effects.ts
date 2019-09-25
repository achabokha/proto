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
   * Load Products effects
   *
   * @memberof CatalogEffects
   */
  @Effect()
  loadProducts = this.actions$.pipe(
    ofType(catalogActions.LOAD_PRODUCTS),
    mergeMap(() => this.productService.fetchProducts()),
    map(products => new catalogActions.LoadProductsSuccess(products)),
    catchError(err =>
      of(new catalogActions.LoadProductsFail({ error: err.message }))
    )
  );

  @Effect()
  getProduct = this.actions$.pipe(
    ofType(catalogActions.VIEW_PRODUCT),
    map((action: catalogActions.ViewProduct) => action.payload),
    mergeMap(selectedProduct => this.productService.getProduct(selectedProduct)),
    map(product => new catalogActions.ViewProductSuccess(product)),
    catchError(err =>
      of(new catalogActions.ViewProductFail({ error: err.message }))
    )
  );

  @Effect({ dispatch: false })
  addProductToCart = this.actions$.pipe(
    ofType(catalogActions.ADD_PRODUCT_TO_CART),
    map((action: catalogActions.AddToCart) => action.payload),
    mergeMap(product => of(this.cartService.add(product))),
    tap(() => this.router.navigate(['/cart/content']))
  );
}
