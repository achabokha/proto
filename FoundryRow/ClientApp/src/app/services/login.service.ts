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

  login(userName: string, userPasw: string): Observable<any> {
    if (userName && userPasw) {
      const responseObs = of({ userName, email: `${userName}@taiga.com` });
      return responseObs;
    }
    throwError('Something wen terrible wrong');
  }
}
