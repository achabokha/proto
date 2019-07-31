import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { AuthService } from '../../../services/auth.service';
import { NavbarService } from '../../../services/navbar.service';
import { DataService } from '../../../services/data.service';
import { LoginModel } from '../../../models/security/login-model';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

    private _subscription: any;
    public isUserLoggedIn: boolean = false;

    user: LoginModel = { username: '', password: '' };

    showError: boolean = false;
    loginning: boolean = false;
    public isCollapsed = true;

    isIn = false;

    // TODO: collapsible 
    // how to make navbar collapsible with Angular 6 --
    // https://angularfirebase.com/lessons/bootstrap-4-collapsable-navbar-work-with-angular/
    // another idia is to use ngb package --
    constructor(public router: Router,
        public authService: AuthService,
        public nav: NavbarService,
        public data: DataService) { }

    ngOnInit(): void {
        this.isUserLoggedIn = this.authService.isLoggedIn;
        this._subscription = this.authService.loginStatusChange.subscribe((value) => this.isUserLoggedIn = value);
    }

    logout() {
        this.isCollapsed = true;
        this.data.cleanAll();
        this.authService.logout();
    }

    login() {
        this.loginning = true;
        this.authService.login(this.user.username, this.user.password)
            .subscribe(r => {
                this.loginning = false;
                this.router.navigate(['dashboard'])
            }, (error: any) => {
                console.error(error);
                this.loginning = false;
                this.authService.isLoginError = true;
                this.router.navigate(['/profile/login'])
            }, () => {
                this.loginning = false;
                this.isCollapsed = true;
            });
    }

    ngOnDestroy() {
        this._subscription.unsubscribe();
    }

    toggleMenu() {
        this.isCollapsed = !this.isCollapsed;
    }


}
