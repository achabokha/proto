import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { UserListComponent } from './user-list/user-list.component';
import { UserService } from '../services/user.service';
import { MaterialModule } from '../material-modules';
import { EditUserDialogComponent } from './edit-user-dialog/edit-user-dialog.component';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [UserListComponent, EditUserDialogComponent],
  imports: [
    CommonModule,
    UserRoutingModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule
  ],
  providers: [UserService],
  entryComponents: [EditUserDialogComponent]
})
export class UserModule { }
