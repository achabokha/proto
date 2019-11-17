import { Router } from '@angular/router';
import { Component } from '@angular/core';

@Component({
    selector: 'ng-shop-cart-empty',
    template: `
    <div class="empty-cart">
    Your  <mat-icon>shopping_cart</mat-icon> is empty
    </div>
  `,
    styles: [
        `
    .empty-cart {
        text-align: center;
        white-space: nowrap;
        color: #757575;
        padding-top: 20px;
        padding-bottom: 20px;
    }
    .empty-cart .cart {
        font-size: 24px !important;
        margin-top: 2px;
    }
    
    `
    ]
})
export class CartEmptyComponent {


}
