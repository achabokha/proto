import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { TransactionsComponent } from './transactions.component';
import { LoadsComponent } from './loads/loads.component';
import { AffiliateComponent } from './affiliate/affiliate.component';
import { AllComponent } from './all/all.component';

const routes: Routes = [
    {
        path: "",
        component: TransactionsComponent,
        children: [
            { path: "loads", component: LoadsComponent },
            { path: "affiliate", component: AffiliateComponent },
            { path: "all", component: AllComponent },
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class TransactionsRoutingModule {
}
