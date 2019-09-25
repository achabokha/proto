import { ThemeStorage } from "./theme-picker/theme-storage/theme-storage";
import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NavComponent } from "./nav/nav.component";
import { LayoutModule } from "@angular/cdk/layout";

import { ThemePickerModule } from "./theme-picker";

import { LocationDataService } from "./services/location-data.service";

import { IonicModule, IonicRouteStrategy } from "@ionic/angular";
import { SplashScreen } from "@ionic-native/splash-screen/ngx";
import { StatusBar } from "@ionic-native/status-bar/ngx";
import { RouteReuseStrategy } from "@angular/router";
import { ChartModule } from "./chart/chart.module";
import { DashboardModule } from "./dashboard/dashboard.module";
import { LoginService } from "./services/login.service";
import { LoginModule, UserProvidedConfigToken } from "./login/login.module";
import { FormsModule } from "@angular/forms";
import { FingerprintAIO } from "@ionic-native/fingerprint-aio/ngx";
import { AuthService } from "./services/auth.service";
import { HttpClientModule } from "@angular/common/http";

import { MatPasswordStrengthModule } from "@angular-material-extensions/password-strength";
import { AuthProcessService } from "./services/auth-process.service";
import { CommentsComponent } from "./comments/comments.component";
import { CommentformComponent } from "./comments/commentform/commentform.component";
import { MaterialModule } from "./material-modules";
import { YtCommentsComponent } from "./yt-comments/yt-comments.component";
import { YtcSpinnerComponent } from "./yt-comments/ytc-spinner/ytc-spinner.component";
import { YtcItemSectionComponent } from "./yt-comments/ytc-item-section/ytc-item-section.component";
import { YtcHeaderComponent } from "./yt-comments/ytc-header/ytc-header.component";
import { YtcThreadComponent } from "./yt-comments/ytc-thread/ytc-thread.component";
import { YtcRepliesComponent } from "./yt-comments/ytc-replies/ytc-replies.component";
import { Facebook } from "@ionic-native/facebook/ngx";
import { GooglePlus } from "@ionic-native/google-plus/ngx";
import { LocalNotifications } from "@ionic-native/local-notifications/ngx";
import { StoreRouterConnectingModule, RouterStateSerializer } from '@ngrx/router-store';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { CoreEffects } from './shop/core/effects/core.effects';
import { environment } from 'src/environments/environment';
import { reducers, metaReducers } from './shop/core/reducers';

import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { CustomRouterStateSerializer } from './shop/shared/utils/router-utils';
import { ShopModule } from './shop/shop.module';
import { CartService } from './shop/core/services/cart.service';
import { ProductService } from './shop/core/services/product.service';


@NgModule({
    declarations: [
        AppComponent,
        NavComponent,
        CommentsComponent,
        CommentformComponent,
        YtCommentsComponent,
        YtcSpinnerComponent,
        YtcItemSectionComponent,
        YtcHeaderComponent,
        YtcThreadComponent,
        YtcRepliesComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        HttpClientModule,
        IonicModule.forRoot(),
        StoreModule.forRoot(reducers, { metaReducers }),
        StoreRouterConnectingModule.forRoot(),

        EffectsModule.forRoot([CoreEffects]),

        !environment.production ? StoreDevtoolsModule.instrument() : [],
        AppRoutingModule,
        BrowserAnimationsModule,
        LayoutModule,
        MaterialModule,
        ThemePickerModule,
        DashboardModule,
        ChartModule,
        LoginModule,
        ShopModule,
        MatPasswordStrengthModule.forRoot()
    ],
    providers: [
        StatusBar,
        CartService,
        ProductService,
        LocalNotifications,
        SplashScreen,
        Facebook,
        GooglePlus,
        { provide: RouteReuseStrategy, useClass: IonicRouteStrategy },
        ThemeStorage,
        LocationDataService,
        LoginService,
        FingerprintAIO,
        AuthService,
        HttpClientModule,
        AuthProcessService,
        { provide: RouterStateSerializer, useClass: CustomRouterStateSerializer },
        { provide: UserProvidedConfigToken, useValue: {} }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
