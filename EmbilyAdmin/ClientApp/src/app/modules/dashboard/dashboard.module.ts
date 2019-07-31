import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { SharedModule } from '../shared/shared.module';
import { DataService } from '../../services/data.service';
import { ChartsModule } from 'ng2-charts-x';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbModule,
        DashboardRoutingModule,
        SharedModule,
        ChartsModule
    ],
    declarations: [
        DashboardComponent
    ]
})
export class DashboardModule { }
