import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ShopRoutingModule } from './shop-routing.module';
import { NgShopComponent } from './core/containers/ng-shop-app.component';
import { LogoComponent } from './core/components/logo.component';
import { CartComponent } from './core/components/cart.component';
import { MenuComponent } from './core/components/menu.component';
import { FooterComponent } from './core/components/footer.component';
import { MaterialModule } from '../material-modules';

@NgModule({
  declarations: [ NgShopComponent, LogoComponent, CartComponent, MenuComponent, FooterComponent],
  imports: [
    MaterialModule,
    CommonModule,
    ShopRoutingModule
  ]
})
export class ShopModule { }
