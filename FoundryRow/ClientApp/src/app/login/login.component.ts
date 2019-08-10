import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/app/services/login.service';
import { FingerprintAIO } from '@ionic-native/fingerprint-aio/ngx';
import { Router } from '@angular/router';

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
        private loginService: LoginService,
        public faio: FingerprintAIO,
        private router: Router) { }

    ngOnInit() {
        this.loginService.isLoggedIn.subscribe((resp: boolean) => {
            if (resp) {
                this.router.navigate(['/dashboard']);
            }
        });
    }

    public showFingerprintModal() {
        if (this.username === '') {
            this.loginError('Please enter login information');
        } else {
            this.faio.show({
                clientId: 'XXXXX', // Android: Used for encryption. iOS: used for dialogue if no `localizedReason` is given.
                clientSecret: 'XXXXX', // Necessary for Android encrpytion of keys. Use random secret key.
                disableBackup: true,  // Only for Android(optional)
                localizedFallbackTitle: 'Use Pin', // Only for iOS
                localizedReason: 'Please prove ownership of phone' // Only for iOS
            })
                .then((result: any) => {
                    if (result === 'Success') {
                        this.loginSuccess({ name: 'testUser', email: 'test@test.com' });
                    } else {
                        this.loginError(result);
                    }
                    console.log('Reponse: ' + result);
                })
                .catch((error: any) => {
                    this.loginError(error);
                });
        }
    }

    loginSuccess(result: any) {
        this.loginService.onLoggedInOut.next(true);
        this.loginService.username = this.username;
        this.loginService.name = result.name;
        this.loginService.email = result.email;

        this.router.navigate(['/dashboard']);
    }

    loginError(error: string) {
        this.errorMessage = error;
    }

    signUp() {
        this.router.navigate(['/login/sign-up']);
    }

    login() {
        if (this.username === '' || this.password === '') {
            this.loginError('Please enter login information');
        } else {
            this.loginService.login(this.username, this.password).subscribe(
                (result: any) => {

                    this.loginSuccess(result);

                },
                error => {
                    this.loginError(error);
                }
            );
        }
    }
}
