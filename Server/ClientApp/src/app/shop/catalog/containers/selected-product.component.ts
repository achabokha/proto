import { Router } from "@angular/router";
import { ProductNav } from "./../models/product-nav";
import { Store } from "@ngrx/store";
import { Observable } from "rxjs";
import { Product } from "../models/product";
import { ActivatedRoute } from "@angular/router";
import { Component, OnInit, ChangeDetectionStrategy } from "@angular/core";
import * as catalogActions from "../actions/catalog.actions";
import * as fromProduct from "../reducers";

@Component({
    selector: 'ng-shop-selected-product',
    template: `
    <div class="selected-product" *ngIf="selectedProduct$ | async as product">
    <ng-shop-product-navigator [current]="(current$ | async) + 1" [count]="count$ | async" (onNext)="nextProduct($event)" (onPrevious)="previousProduct($event)"></ng-shop-product-navigator>

    <table *ngIf="product">
      <td width="50%">
        <img height="70%" [src]="'assets/img/' + product.imageUrl">
      </td>
      <td width="50%">
        <h1>{{product.title}}</h1>
        <p> By {{product.author}} </p>
        <div class="price">{{product.price | currency:'EUR':true }}</div>
        <p> {{product.pages}} pages</p>
        <div [innerHtml]="product.description"></div>
        <button class="add-cart" (click)="addProductToCart(product)" md-raised-button color="primary">ADD TO CART <i class="material-icons">add_shopping_cart</i></button>
      </td>
    </table>
    <p class="review-title">Reviews</p>
    <app-reviews-container [productId]="product.id"></app-reviews-container>

  </div>
    `,
    styles: [
        `
    :host{
        bottom:50px;
    }

    .detail {
        margin: 64px 32px;
        width: 50%;
        max-width: 400px;
        transition: opacity 0.4s;
        opacity: 0;
    }

    .block {
        display: inline-block;
    }

    app-reviews-container{
        margin-left: 15%;
        margin-right: 15%;
    }

    md-grid-list{
        display:block !important;
    }

    .review-title{
        -webkit-tap-highlight-color: rgba(0, 0, 0, 0);
        color: #202020;
        font-size: 1.3em;
        font-weight: 500;
        margin: 32px 0;
    }

    .selected-product {
        margin-left : 10%;
        margin-right: 10%;
        text-align: center;
    }

    td,th {
        vertical-align: middle !important;
    }

    .add-cart{
       margin-top:30px;
    }
        `
    ],
    changeDetection: ChangeDetectionStrategy.OnPush

})

export class SelectedProductComponent implements OnInit {
    productId: string;
    selectedProduct$: Observable<Product>;
    loading$: Observable<boolean>;

    current$: Observable<number>;
    count$: Observable<number>;
    nextId: string;
    previousId: string;

    constructor(private store: Store<fromProduct.CatalogState>, private route: ActivatedRoute,
                private router: Router) {
        this.selectedProduct$ = this.store.select(fromProduct.getSelectedProduct);
        this.loading$ = this.store.select(fromProduct.getProductLoading);
        this.current$ = this.store.select(fromProduct.getCurrent);
        this.count$ = this.store.select(fromProduct.getTotal);

        // subscribe to nextId and previousId changes
        this.store.select(fromProduct.getNextId).subscribe(nextId => {
            this.nextId = nextId;
        });

        this.store.select(fromProduct.getPreviousId).subscribe(previousId => {
            this.previousId = previousId;
        });
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.productId = params["id"];
            this.store.dispatch(new catalogActions.ViewProduct(this.productId));
        });
    }

    nextProduct() {
        console.log("on next product");
        if (this.nextId) {
            this.router.navigate(['/catalog/product', this.nextId]);
        }

    }


    previousProduct() {
        console.log("on previous product");
        if (this.previousId) {
            this.router.navigate(['/catalog/product', this.previousId]);
        }
    }

    addProductToCart(product: Product) {
        this.store.dispatch(new catalogActions.AddToCart(product));
    }
}
