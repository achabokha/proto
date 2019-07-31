import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { ProgramsComponent } from './programs.component';
import { AuthGuard } from '../../services/auth.guard';
import { ProgramDetailsComponent } from './programDetails/programDetails.component';

const routes: Routes = [
    {
        path: "",
        //component: AccountsComponent,
        children: [
            { path: '', component: ProgramsComponent },
            { path: ':programId', component: ProgramDetailsComponent, canActivate: [AuthGuard] },   
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class ProgramsRoutingModule {
}
