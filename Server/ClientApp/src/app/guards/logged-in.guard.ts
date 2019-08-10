import {Inject, Injectable} from '@angular/core'
import {CanActivate, Router} from '@angular/router'
import {map} from 'rxjs/operators'
import {Observable} from 'rxjs'
import { NgxAuthFirebaseUIConfig } from '../interfaces/config.interface';
import { AuthProcessService } from '../services/auth-process.service';
import { NgxAuthFirebaseUIConfigToken } from '../login/login.module';


@Injectable({
  providedIn: 'root',
})
export class LoggedInGuard implements CanActivate {
  constructor(
    @Inject(NgxAuthFirebaseUIConfigToken)
    private config: NgxAuthFirebaseUIConfig,
    private router: Router,
    private auth: AuthProcessService,
  ) {
  }

  canActivate(): Observable<boolean> {
    return this.auth.afa.user.pipe(
      map(res => {
        if (res) {
          if (this.config.authGuardLoggedInURL && this.config.authGuardLoggedInURL !== '/') {
            this.router.navigate([`${this.config.authGuardLoggedInURL}`]);
          }
          return true
        }
        if (this.config.authGuardFallbackURL && this.config.authGuardFallbackURL !== '/') {
          this.router.navigate([`${this.config.authGuardFallbackURL}`]);
        }
        return false
      }),
    )
  }
}