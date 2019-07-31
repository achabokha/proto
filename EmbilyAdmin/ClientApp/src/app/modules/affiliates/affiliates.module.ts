import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { AffiliatesRoutingModule } from './affiliates-routing.module';
import { AffiliatesComponent } from './affiliates.component';
import { SharedModule } from '../shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { AffiliateComponent } from './affiliate/affiliate.component';
import { InvitationsComponent } from './invitations/invitations.component';
import { TokensComponent } from './tokens/tokens.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        NgbModule, // bootstrap helpers
        AffiliatesRoutingModule,
        NgxDatatableModule,
        SharedModule
    ],
    declarations: [
        AffiliatesComponent,
        InvitationsComponent,
        AffiliateComponent,
        TokensComponent
        
    ],
    providers: []
})
export class AffiliatesModule { }
