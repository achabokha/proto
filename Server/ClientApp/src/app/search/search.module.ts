import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SearchRoutingModule } from './search-routing.module';
import { SearchComponent } from './search.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CustomMaterialModuleModule } from '../custom-material-module/custom-material-module.module';



@NgModule({
  declarations: [SearchComponent],
  imports: [
    CommonModule,
    CustomMaterialModuleModule,
    SearchRoutingModule,
    FlexLayoutModule
  ]
})
export class SearchModule { }
