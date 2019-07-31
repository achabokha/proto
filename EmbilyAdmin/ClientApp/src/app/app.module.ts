import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule, HttpClient } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from "./app-routing.module";
import { HomeModule } from './modules/home/home.module';
import { DataService } from './services/data.service';
import { NavbarService } from './services/navbar.service';
import { AuthService } from './services/auth.service';
import { AuthGuard } from './services/auth.guard';
import { SignUpService } from './services/signup.service';
import { UserService } from './services/user.service';
import { DashboardService } from './services/dashboard.service';
import { ApplicationService } from './services/application.service';
import { AccountsService } from './services/accounts.service';
import { AffiliateService } from './services/affiliate.service';
import { RoleService } from './services/roles.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ProgramsService } from './services/programs.service';
import { httpInterceptorProviders, httpInterceptorProvidersProd } from './http-interceptors';
import { MessageService } from './services/message.service';
import { environment } from 'src/environments/environment';

export const createTranslateLoader = (http: HttpClient) => {
    /* for development
    return new TranslateHttpLoader(
        http,
        '/start-angular/SB-Admin-BS4-Angular-6/master/dist/assets/i18n/',
        '.json'
    ); */
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
};

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        FormsModule,
        RouterModule,
        NgbModule.forRoot(), // bootstrap helpers
        AppRoutingModule,
        HomeModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: createTranslateLoader,
                deps: [HttpClient]
            }
        }),
    ],
    providers: [
        DataService,
        NavbarService,
        AuthService,
        AuthGuard,
        SignUpService,
        UserService,
        DashboardService,
        ApplicationService,
        AccountsService,
        AffiliateService,
        RoleService,
        ProgramsService,
        MessageService,
        (environment.production) ? httpInterceptorProvidersProd : httpInterceptorProviders
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
