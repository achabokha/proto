import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginService } from '../services/login.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {
  constructor(private loginService: LoginService, private router: Router) { }

  canActivate(next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {

    console.log('Guard function has been invoked');

    let authenticated = false;

    if (this.loginService.userId) {
      authenticated = true;
    } else {
      this.router.navigate(['/login']);
    }
    console.log('Returning from Guard function with: ' + authenticated);
    return authenticated;
  }

}
