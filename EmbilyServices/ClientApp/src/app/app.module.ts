import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AppRoutingModule } from "./app-routing.module";
import { HomeModule } from './modules/home/home.module';
import { SharedModule } from './modules/shared/shared.module';
import { DataService } from './services/data.service';
import { NavbarService } from './services/navbar.service';
import { AuthService } from './services/auth.service';
import { AuthGuard } from './services/auth.guard';
import { AccountsService } from './services/accounts.service';
import { AffiliateService } from './services/affiliate.service';
import { BitcoinService } from './services/bitcoin.service';
import { EthereumService } from './services/ethereum.service';
import { LitecoinService } from './services/litecoin.service';
import { SignUpService } from './services/signup.service';
import { UserService } from './services/user.service';
import { ApplicationService } from './services/application.service';
import { TickerService } from './services/ticker.service';
import { BlockchaininfoService } from './services/blockchaininfo.service';
import { RecaptchaModule } from 'ng-recaptcha';

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        FormsModule,
        RouterModule,
        NgbModule.forRoot(), // bootstrap helpers
        AppRoutingModule,
        HomeModule,
        RecaptchaModule.forRoot(),
    ],
    providers: [
        DataService,
        NavbarService,
        AuthService,
        AuthGuard,
        AccountsService,
        AffiliateService,
        BitcoinService,
        EthereumService,
        LitecoinService,
        SignUpService,
        UserService,
        ApplicationService,
        BlockchaininfoService,
        TickerService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}

