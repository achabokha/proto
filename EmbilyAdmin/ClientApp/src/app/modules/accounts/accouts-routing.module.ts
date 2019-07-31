import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { AccountsComponent } from './accounts.component';
import { AuthGuard } from '../../services/auth.guard';
import { AccountDetailsComponent } from './accountDetails/accountDetails.component';

const routes: Routes = [
    {
        path: "",
        //component: AccountsComponent,
        children: [
            { path: '', component: AccountsComponent },
            { path: ':accountId', component: AccountDetailsComponent, canActivate: [AuthGuard] },   
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class AccoutsRoutingModule {
}
