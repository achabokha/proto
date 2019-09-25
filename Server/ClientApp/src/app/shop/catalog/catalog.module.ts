import { CatalogListComponent } from './containers/catalog-list.component';
import { CatalogRootComponent } from './containers/catalog-root.component';
import { CatalogItemComponent } from './components/catalog-item.component';
import { ReviewModule } from './../review/review.module';
import { SharedModule } from './../shared/shared.module';
import { CatalogEffects } from './effects/catalog.effects';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { CatalogService } from './services/products.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CatalogRoutingModule } from './catalog-routing.module';
import { SelectedProductComponent } from './containers/selected-product.component';

import { reducers } from './reducers';
import { ProductNavigatorComponent } from './components/product-navigator/product-navigator.component';
import { AddToCartComponent } from './components/add-to-cart.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  imports: [
    CommonModule,
    SharedModule,
    CatalogRoutingModule,
    ReviewModule,
    HttpClientModule,
    StoreModule.forFeature('catalog', reducers),
    EffectsModule.forFeature([CatalogEffects])

  ],
  providers: [CatalogService],
  declarations: [SelectedProductComponent, CatalogListComponent,
    CatalogRootComponent, ProductNavigatorComponent,
    AddToCartComponent, CatalogItemComponent]
})
export class CatalogModule { }
