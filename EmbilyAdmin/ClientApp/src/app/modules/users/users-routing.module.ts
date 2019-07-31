import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { UsersComponent } from './users.component';
import { UserDetailsComponent } from './userDetails/userDetails.component';
import { AuthGuard } from '../../services/auth.guard';
import { RolesComponent } from './roles/roles.component';

const routes: Routes = [
    {
        path: "",
        //component: UsersComponent,
        children: [
            { path: '', component: UsersComponent },
            { path: 'details/:userId', component: UserDetailsComponent, canActivate: [AuthGuard] },
            { path: 'roles', component: RolesComponent, canActivate: [AuthGuard] },
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class UsersRoutingModule {
}
