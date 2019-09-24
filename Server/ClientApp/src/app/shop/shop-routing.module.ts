import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NgShopComponent } from './core/containers/ng-shop-app.component';


const routes: Routes = [
  {
    path: "",
    component: NgShopComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShopRoutingModule { }
