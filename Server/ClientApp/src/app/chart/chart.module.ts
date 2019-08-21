import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChartRoutingModule } from './chart-routing.module';
import { ChartComponent } from './chart.component';
import { MaterialModule } from '../material-modules';


@NgModule({
  declarations: [ ChartComponent ],
  imports: [
    CommonModule,
    MaterialModule,
    ChartRoutingModule
  ]
})
export class ChartModule { }
