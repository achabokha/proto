import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';

import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { DataService } from '../../services/data.service';
import { UsersRoutingModule } from './users-routing.module';
import { UsersComponent } from './users.component';
import { UserDetailsComponent } from './userDetails/userDetails.component';
import { RolesComponent } from './roles/roles.component';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbModule,
        UsersRoutingModule,
        SharedModule,
        RouterModule,
        NgxDatatableModule,
    ],
    declarations: [
        UsersComponent,
        UserDetailsComponent,
        RolesComponent,
    ]
})
export class UsersModule { }
