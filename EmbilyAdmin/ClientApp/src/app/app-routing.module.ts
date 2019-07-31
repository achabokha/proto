import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { AppComponent } from './app.component';
import { AuthGuard } from './services/auth.guard';


const routes: Routes = [
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    { path: 'login', redirectTo: 'profile/login', pathMatch: 'prefix' },
    { path: 'signup', redirectTo: 'profile/signup', pathMatch: 'prefix' },
    { path: 'confirmemail', redirectTo: 'profile/confirmemail', pathMatch: 'prefix' },
    { path: 'resetpassword', redirectTo: 'profile/resetpassword', pathMatch: 'prefix' },
    {
        path: "dashboard",
        canActivate: [AuthGuard],
        loadChildren: "./modules/dashboard/dashboard.module#DashboardModule",
        data: { preload: true }
    },   
    {
        path: "applications",
        canActivate: [AuthGuard],
        loadChildren: "./modules/applications/applications.module#ApplicationsModule",
        data: { preload: true }
    },
    {
        path: "users",
        canActivate: [AuthGuard],
        loadChildren: "./modules/users/users.module#UsersModule",
        data: { preload: true }
    },
    {
        path: "accounts",
        canActivate: [AuthGuard],
        loadChildren: "./modules/accounts/accounts.module#AccountsModule",
        data: { preload: true }
    },
    {
        path: "transactions",
        canActivate: [AuthGuard],
        loadChildren: "./modules/transactions/transactions.module#TransactionsModule",
        data: { preload: true }
    },
    {
        path: "affiliates",
        canActivate: [AuthGuard],
        loadChildren: "./modules/affiliates/affiliates.module#AffiliatesModule",
        data: { preload: true }
    },
    {
        path: "programs",
        canActivate: [AuthGuard],
        loadChildren: "./modules/programs/programs.module#ProgramsModule",
        data: { preload: true }
    },
    {
        path: "profile",
        loadChildren: "./modules/profile/profile.module#ProfileModule",
        data: { preload: true }
    },
    
];


@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: [] // don't even think to remove --
})
export class AppRoutingModule {
}
