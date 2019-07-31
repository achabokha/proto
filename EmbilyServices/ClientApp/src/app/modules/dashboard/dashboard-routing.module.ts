import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";

import { DashboardComponent } from './dashboard.component';
import { LoadComponent } from './load/load.component';
import { AffiliateInviteComponent } from './affiliate-invite/affiliate-invite.component';

const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', component: DashboardComponent },
            { path: 'load/:currencyCode/:accountId', component: LoadComponent },
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class DashboardRoutingModule {
}
