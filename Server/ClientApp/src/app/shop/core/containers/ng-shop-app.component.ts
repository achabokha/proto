import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { Component, OnInit } from '@angular/core';

import * as fromCore from '../reducers';
import * as actions from '../actions/core.actions';

@Component({
  selector: 'ng-shop-root',
  template: `
    <ng-shop-cart></ng-shop-cart>
    <ng-shop-menu></ng-shop-menu>
    <router-outlet></router-outlet>
  `
})
export class NgShopComponent implements OnInit {
  //cartCount$:Observable<number>;

  constructor(private store: Store<fromCore.State>) {
    //this.cartCount$ = this.store.select(fromCore.getCartCount);
  }

  ngOnInit(): void {
    //this.store.dispatch(new actions.LoadCartContent());
  }
}
