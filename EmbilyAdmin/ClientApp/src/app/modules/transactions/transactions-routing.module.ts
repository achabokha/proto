import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { TransactionsComponent } from './transactions.component';

const routes: Routes = [
    {
        path: "",
        component: TransactionsComponent,
        children: [
            //{ path: "loads", component: LoadsComponent },
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class TransactionsRoutingModule {
}
