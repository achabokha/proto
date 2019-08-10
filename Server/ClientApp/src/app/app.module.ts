import { ThemeStorage } from './theme-picker/theme-storage/theme-storage';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ThemePickerModule } from './theme-picker';

import { MatRadioModule } from '@angular/material/radio';
import { MatDialogModule } from "@angular/material";
import { LocationDataService } from './services/location-data.service';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';
import { SplashScreen } from '@ionic-native/splash-screen/ngx';
import { StatusBar } from '@ionic-native/status-bar/ngx';
import { RouteReuseStrategy } from '@angular/router';
import { ChartModule } from './chart/chart.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { CustomMaterialModuleModule } from './custom-material-module/custom-material-module.module';
import { LoginService } from './services/login.service';
import { LoginModule } from './login/login.module';
import { FormsModule } from '@angular/forms';
import { FingerprintAIO } from '@ionic-native/fingerprint-aio/ngx';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent],
   imports: [
      BrowserModule,
      FormsModule,
      IonicModule.forRoot(),
      AppRoutingModule,
      BrowserAnimationsModule,
      LayoutModule,
      CustomMaterialModuleModule,
      ThemePickerModule,
      DashboardModule,
      ChartModule,
      LoginModule
   ],
   providers: [
      StatusBar,
      SplashScreen,
      { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
      ThemeStorage,
      LocationDataService,
      LoginService,
      FingerprintAIO
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
