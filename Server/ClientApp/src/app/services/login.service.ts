import { Injectable } from '@angular/core';
import { Observable, of, Observer, throwError } from 'rxjs';
import { EventEmitter } from '@angular/core';
import { AuthService } from './auth.service';

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

  constructor(private authService: AuthService) { }

  login(userName: string, userPasw: string): any {
    // if (userName && userPasw) {
    //   return this.authService.login(userName, userPasw);
    // }



    throwError('Something wen terrible wrong');
  }
}
