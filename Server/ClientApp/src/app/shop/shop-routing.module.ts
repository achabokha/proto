import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NgShopComponent } from './core/containers/ng-shop-app.component';


const routes: Routes = [
  {
    path: "",
    component: NgShopComponent
  },
  {
    path: "catalog",
    loadChildren: () => import('./catalog/catalog.module').then(mod => mod.CatalogModule)
  },
  {
    path: "cart",
    loadChildren: () => import('./cart/cart.module').then(mod => mod.CartModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ShopRoutingModule { }
