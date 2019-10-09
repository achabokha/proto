import { Router, ActivatedRoute } from "@angular/router";
import { Component, OnInit } from "@angular/core";
import { Store, select } from "@ngrx/store";
import * as fromRouter from "./../reducers";
import { map } from 'rxjs/operators';

@Component({
  selector: 'ng-shop-menu',
  template: `
    <nav mat-tab-nav-bar aria-label="weather navigation links">
      <a
        mat-tab-link
        *ngFor="let routeLink of routeLinks; let i = index"
        [routerLink]="routeLink.link"
        [active]="activeLinkIndex === i"
        (click)="activeLinkIndex = i"
      >
        {{ routeLink.label }}
      </a>
    </nav>
  `,
  styles: [
    `
      :host {
        margin-top: 30px;
      }
    `
  ]
})
export class MenuComponent implements OnInit {
  routeLinks: any[];
  activeLinkIndex = 0;
  constructor(private router: Router,
    private route: ActivatedRoute,
    private store: Store<fromRouter.State>, ) {
    this.routeLinks = [
      { label: "Catalog", link: "/catalog/list" },
      { label: "Cart", link: "/cart/content" },
      { label: "Order", link: "/cart/order" }
    ];

  }

  ngOnInit(): void {
    this.store
      .select(state => state.routerReducer)
      .pipe(map(d => d.state.url))
      .subscribe(d => {
        switch (d) {
          case "/catalog/list":
            this.activeLinkIndex = 0;
            break;
          case "/cart/content":
            this.activeLinkIndex = 1;
            break;
          case "/cart/order":
            this.activeLinkIndex = 2;
            break;
          default:
            break;
        }
      })
  }
}
