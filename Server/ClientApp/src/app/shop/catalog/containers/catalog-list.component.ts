import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import * as fromCollection from './../reducers';
import { Store } from '@ngrx/store';
import { Product } from './../models/product';
import { Observable } from 'rxjs';
import * as catalogActions from './../actions/catalog.actions';

@Component({
  selector: 'ng-shop-catalog-list',
  template: `
    <div class="catalog-container">
      <div *ngIf="(products$ | async) as products" class="catalog-list">
      {{ product | json }}
        <app-catalog-item
          *ngFor="let product of products"
          [product]="product"
          (addToCart)="addProductToCart($event)"
          (productDetails)="goToDetails($event)"
        ></app-catalog-item>
      </div>
      <mat-progress-spinner
        *ngIf="(loading$ | async)"
        color="primary"
        mode="indeterminate"
        value="50"
      >
      </mat-progress-spinner>
    </div>
  `,
  styles: [
    `
      .catalog-container {
        margin-left: 10%;
        margin-right: 10%;
        text-align: center;
      }

      .catalog-list {
        display: flex;
        flex-wrap: wrap;
        justify-content: center;
      }
      mat-progress-spinner {
        margin: 0 auto;
      }
    `
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CatalogListComponent implements OnInit {
  products$: Observable<Product[]>;
  loading$: Observable<boolean>;

  constructor(
    private store: Store<fromCollection.State>,
    private router: Router
  ) {
    this.products$ = this.store.select(fromCollection.getCatalogContent);
    this.loading$ = this.store.select(fromCollection.getCatalogLoading);
  }

  ngOnInit() {
    // Dispatch the load action
    this.store.dispatch(new catalogActions.LoadProducts());
  }

  viewProduct(productId) {
    this.store.dispatch(new catalogActions.ViewProduct(productId));
  }

  addProductToCart(product: Product) {
    this.store.dispatch(new catalogActions.AddToCart(product));
  }

  goToDetails(product: Product) {
    this.router.navigate(['/catalog/product/', product.id]);
  }
}
