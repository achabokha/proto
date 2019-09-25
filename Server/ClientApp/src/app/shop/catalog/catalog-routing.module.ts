import { CatalogListComponent } from "./containers/catalog-list.component";
import { CatalogRootComponent } from "./containers/catalog-root.component";
import { SelectedProductComponent } from "./containers/selected-product.component";
import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

const routes: Routes = [
  {
    path: "", component: CatalogListComponent,
  },
  { path: "list", component: CatalogListComponent },
  { path: "product/:id", component: SelectedProductComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CatalogRoutingModule { }
