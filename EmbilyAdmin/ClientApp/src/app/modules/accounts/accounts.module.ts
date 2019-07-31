import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { DataService } from '../../services/data.service';
import { AccoutsRoutingModule } from './accouts-routing.module';
import { AccountsComponent } from './accounts.component';
import { AccountDetailsComponent } from './accountDetails/accountDetails.component';



@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbModule,
        AccoutsRoutingModule,
        SharedModule,
        RouterModule,
        NgxDatatableModule,
    ],
    declarations: [
        AccountsComponent,
        AccountDetailsComponent
    ]
})
export class AccountsModule { }
