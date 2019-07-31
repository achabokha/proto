import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { SharedModule } from '../shared/shared.module';


import { ApplicationComponent } from './application/application.component';
import { CardComponent } from './card/card.component';
import { AffiliateAccountsComponent } from './affiliate-accounts/affiliate-accounts.component';
import { AffiliateInviteComponent } from './affiliate-invite/affiliate-invite.component';
import { AffiliateTokensComponent } from './affiliate-tokens/affiliate-tokens.component';
import { CryptonewsComponent } from './cryptonews/cryptonews.component';
import { BlockchaininfoComponent } from './blockchaininfo/blockchaininfo.component';
import { LoadComponent } from './load/load.component';
import { LoadLimitsLiveComponent } from './load-limits-live/load-limits-live.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbModule,
        DashboardRoutingModule,
        SharedModule,
    ],
    declarations: [
        DashboardComponent,
        ApplicationComponent,
        CardComponent,
        AffiliateAccountsComponent,
        AffiliateInviteComponent,
        AffiliateTokensComponent,
        CryptonewsComponent,
        BlockchaininfoComponent,
        LoadComponent,
        LoadLimitsLiveComponent
    ]
})
export class DashboardModule { }
