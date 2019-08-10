import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { LoginGuard } from './guards/login.guard';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'dashboard',
    canActivate: [LoginGuard],
    loadChildren: () => import('./dashboard/dashboard.module').then(mod => mod.DashboardModule)
  },
  {
    path: 'chart',
    canActivate: [LoginGuard],
    loadChildren: () => import('./chart/chart.module').then(mod => mod.ChartModule)
  },
  {
    path: 'login',
    loadChildren: () => import('./login/login.module').then(mod => mod.LoginModule)
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
