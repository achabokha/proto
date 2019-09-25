import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'ng-shop-catalog-root',
    template: `
        <router-outlet></router-outlet>
    `
})

export class CatalogRootComponent implements OnInit {
    constructor() { }

    ngOnInit() { }
}