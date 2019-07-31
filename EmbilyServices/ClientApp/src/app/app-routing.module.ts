import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { AppComponent } from './app.component';
import { HomeComponent } from './modules/home/home.component';
import { AuthGuard } from './services/auth.guard';


const routes: Routes = [
    //{ path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: '', component: HomeComponent },
    { path: 'login', redirectTo: 'profile/login', pathMatch: 'prefix' },
    { path: 'signup', redirectTo: 'profile/signup', pathMatch: 'prefix' },
    { path: 'confirmemail', redirectTo: 'profile/confirmemail', pathMatch: 'prefix' },
    { path: 'resetpassword', redirectTo: 'profile/resetpassword', pathMatch: 'prefix' },
    { path: 'fees', redirectTo: 'help/fees', pathMatch: 'prefix' },
    { path: 'home', component: HomeComponent },
    { path: 'home/:token', component: HomeComponent },
    {
        path: "dashboard",
        canActivate: [AuthGuard],
        loadChildren: "./modules/dashboard/dashboard.module#DashboardModule",
        data: { preload: true }
    },
    {
        path: "transactions",
        canActivate: [AuthGuard],
        loadChildren: "./modules/transactions/transactions.module#TransactionsModule",
        data: { preload: true }
    },
    {
        path: "application",
        canActivate: [AuthGuard],
        loadChildren: "./modules/application/application.module#ApplicationModule",
        data: { preload: true }
    },
    {
        path: "profile",
        loadChildren: "./modules/profile/profile.module#ProfileModule",
        data: { preload: true }
    },
    {
        path: "legal",
        loadChildren: "./modules/legal-and-help/legal-and-help.module#LegalAndHelpModule",
        data: { preload: false }
    },
    {
        path: "help",
        loadChildren: "./modules/legal-and-help/legal-and-help.module#LegalAndHelpModule",
        data: { preload: false }
    },
];


@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: [] // don't even think to remove --
})
export class AppRoutingModule {
}
