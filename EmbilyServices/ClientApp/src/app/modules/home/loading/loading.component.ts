import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, NavigationStart } from '@angular/router';

@Component({
    selector: 'app-loading',
    templateUrl: './loading.component.html',
    styleUrls: ['./loading.component.scss']
})
export class LoadingComponent implements OnInit {

    showRouterLoad: boolean = true;

    constructor(private router: Router) {
        router.events.subscribe((e) => {

            if (e instanceof NavigationStart) {
                this.showRouterLoad = true;
            }

            if (e instanceof NavigationEnd) {
                this.showRouterLoad = false;
            }

        });
    }

    ngOnInit() {
    }

}
