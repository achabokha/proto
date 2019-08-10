import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChartRoutingModule } from './chart-routing.module';
import { ChartComponent } from './chart.component';
import { CustomMaterialModuleModule } from '../custom-material-module/custom-material-module.module';
import { IonicModule } from '@ionic/angular';


@NgModule({
  declarations: [ ChartComponent ],
  imports: [
    CommonModule,
    CustomMaterialModuleModule,
    ChartRoutingModule
  ]
})
export class ChartModule { }
