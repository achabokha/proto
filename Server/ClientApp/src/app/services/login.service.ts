import { Injectable } from '@angular/core';
import { Observable, of, Observer, throwError } from 'rxjs';
import { EventEmitter } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class LoginService {

    userId: number = null;
    username = '';
    name = '';
    email = '';
    isLoggedIn: Observable<boolean> = of(false);
    onLoggedInOut: Observer<any> = {
        next: x => {
            this.userId = x ? 1 : null;
            this.isLoggedIn = of(x);
        },
        error: err => console.error('Observer got an error: ' + err),
        complete: () => console.log('Observer got a complete notification'),
    };

    constructor() { }

    login(username: string, password: string): Observable<any> {
        if (username && password) {
            const responseObs = of({ username, email: `${username}@taiga.com` });
            return responseObs;
        }
        throwError('Something went terribly wrong!');
    }
}
