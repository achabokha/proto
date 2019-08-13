import { Inject, Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { NgxAuthFirebaseUIConfig } from '../interfaces/config.interface';
import { AuthProcessService, NgxAuthFirebaseUIConfigToken } from '../services/auth-process.service';
import { AuthService } from '../services/auth.service';


@Injectable({
  providedIn: 'root',
})
export class LoggedInGuard implements CanActivate {
  constructor(
    @Inject(NgxAuthFirebaseUIConfigToken)
    private config: NgxAuthFirebaseUIConfig,
    private router: Router,
    private afa: AuthService,
  ) {
  }

  canActivate() {
    return this.afa.currentUser$.pipe(
      map((currentUser:any) => {
        if (currentUser && currentUser.email !== '') {
          if (this.config.authGuardLoggedInURL && this.config.authGuardLoggedInURL !== '/') {
            this.router.navigate([`${this.config.authGuardLoggedInURL}`]);
          }
          return true;
        }
        if (this.config.authGuardFallbackURL && this.config.authGuardFallbackURL !== '/') {
          this.router.navigate([`${this.config.authGuardFallbackURL}`]);
        }
        return false;

      })
    );
  }
}
