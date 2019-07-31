import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { DataService } from '../../services/data.service';
import { ProgramsRoutingModule } from './programs-routing.module';
import { ProgramsComponent } from './programs.component';
import { ProgramDetailsComponent } from './programDetails/programDetails.component';



@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbModule,
        ProgramsRoutingModule,
        SharedModule,
        RouterModule,
        NgxDatatableModule,
    ],
    declarations: [
        ProgramsComponent,
        ProgramDetailsComponent
    ]
})
export class ProgramsModule { }
