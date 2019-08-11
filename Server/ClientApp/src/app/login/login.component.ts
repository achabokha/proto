import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login.service';
import { FingerprintAIO } from '@ionic-native/fingerprint-aio/ngx';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
    username = '';
    password = '';

    errorMessage = '';

    constructor(
        public auth: AuthService,
        private router: Router) { }

    ngOnInit() {
        if (this.auth.isLoggedIn) {
            this.router.navigate(['/dashboard']);
        }
    }

    loginSuccess(result: any) {
        this.router.navigate(['/dashboard']);
    }
}
