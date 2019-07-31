import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { TransactionsRoutingModule } from './transactions-routing.module';
import { TransactionsComponent } from './transactions.component';
import { LoadsComponent } from './loads/loads.component';
import { SharedModule } from '../shared/shared.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { AffiliateComponent } from './affiliate/affiliate.component';
import { AllComponent } from './all/all.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        NgbModule, // bootstrap helpers
        TransactionsRoutingModule,
        NgxDatatableModule,
        SharedModule
    ],
    declarations: [
        TransactionsComponent,
        LoadsComponent,
        AffiliateComponent,
        AllComponent,
        
    ],
    providers: []
})
export class TransactionsModule { }
