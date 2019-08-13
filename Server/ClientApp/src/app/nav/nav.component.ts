import { Component, HostBinding, ViewChild } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { OverlayContainer } from '@angular/cdk/overlay';
import { ThemeStorage } from '../theme-picker/theme-storage/theme-storage';
import { NgZone } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { MatSidenav, MatSidenavContent, MatToolbar } from '@angular/material';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
  animations: [
    trigger('openClose', [
      // ...
      state('open', style({
        height: '200px',
        opacity: 1,
        backgroundColor: 'yellow'
      })),
      state('closed', style({
        height: '100px',
        opacity: 0.5,
        backgroundColor: 'green'
      })),
      transition('open => closed', [
        animate('1s')
      ]),
      transition('closed => open', [
        animate('0.5s')
      ]),
    ]),
  ],
})
export class NavComponent {

  @ViewChild('drawer', { static: false }) sideNav: MatSidenav;
  @ViewChild('drawerToolbar', { static: false }) sideNavToolbar: MatToolbar;

  isOpen = false;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches)
    );
  @HostBinding('class') componentCssClass;
  initThemeName: string;

  constructor(public zone: NgZone,
    private breakpointObserver: BreakpointObserver,
    public overlayContainer: OverlayContainer,
    private themeStorage: ThemeStorage,
    private authService: AuthService,
    private router: Router) {

    // must do theme here, as nav a parent component for the whole app --
    this.componentCssClass = this.initThemeName = this.themeStorage.getStoredThemeName();
    this.onThemeChanged(this.initThemeName);
    this.authService.authState.user.pipe(map(d => {
      console.log(d);
    }));

    this.authService.authState.user.subscribe(d => {
      console.log(d);
    });
  }

  goTo(link: string) {
    if (this.sideNav.opened) {
      this.sideNav.close();
    } else {
      this.sideNav.open();
    }
  }

  logOut() {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }

  onThemeChanged(theme: string) {
    this.overlayContainer.getContainerElement().classList.remove(this.componentCssClass);
    this.overlayContainer.getContainerElement().classList.add(theme);
    this.componentCssClass = theme;
  }

}
