import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../../../services/auth.service';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit, OnDestroy {

    collapedSideBar: boolean;
    private _subscription: any;
    public isUserLoggedIn: boolean = false;

    constructor(public authService: AuthService) { }

    ngOnInit(): void {
        this.isUserLoggedIn = this.authService.isLoggedIn;
        this._subscription = this.authService.loginStatusChange.subscribe((value) => this.isUserLoggedIn = value);
    }

    ngOnDestroy() {
        this._subscription.unsubscribe();
    }

    receiveCollapsed($event) {
        this.collapedSideBar = $event;
    }

}
