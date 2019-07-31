import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { NgForm } from '@angular/forms';

import { AuthService } from '../../../services/auth.service';
import { NavbarService } from '../../../services/navbar.service';
import { DataService } from '../../../services/data.service';
import { LoginModel } from '../../../models/security/login-model';
import { TranslateService } from '@ngx-translate/core';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

    pushRightClass: string = 'push-right';
    private _subscription: any;
    public isUserLoggedIn: boolean = false;

    user: LoginModel = { username: '', password: '' };

    showError: boolean = false;
    loginning: boolean = false;
    public isCollapsed = true;

    isIn = false;

    date: Date = new Date(Date.now());

    Clock = this.date.toUTCString();

    // TODO: collapsible 
    // how to make navbar collapsible with Angular 6 --
    // https://angularfirebase.com/lessons/bootstrap-4-collapsable-navbar-work-with-angular/
    // another idia is to use ngb package --
    constructor(
        public translate: TranslateService,
        public router: Router,
        public authService: AuthService,
        public nav: NavbarService,
        private data: DataService) {

        this.translate.addLangs(['en']);
        this.translate.setDefaultLang('en');
        const browserLang = this.translate.getBrowserLang();
        this.translate.use(browserLang.match(/en/) ? browserLang : 'en');

        this.router.events.subscribe(val => {
            if (
                val instanceof NavigationEnd &&
                window.innerWidth <= 992 &&
                this.isToggled()
            ) {
                this.toggleSidebar();
            }
        });
    }

    ngOnInit(): void {
        setInterval(() => {
            this.date = new Date(Date.now());
            this.Clock = this.date.toUTCString();
        }, 1000);
        this.isUserLoggedIn = this.authService.isLoggedIn;
        this._subscription = this.authService.loginStatusChange.subscribe((value) => this.isUserLoggedIn = value);
    }

    logout() {
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
            });
    }

    ngOnDestroy() {
        this._subscription.unsubscribe();
    }

    isToggled(): boolean {
        const dom: Element = document.querySelector('body');
        return dom.classList.contains(this.pushRightClass);
    }

    toggleSidebar() {
        const dom: any = document.querySelector('body');
        dom.classList.toggle(this.pushRightClass);
    }

    changeLang(language: string) {
        this.translate.use(language);
    }
}
