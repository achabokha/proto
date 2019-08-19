import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { CustomMaterialModuleModule } from '../custom-material-module/custom-material-module.module';
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    CustomMaterialModuleModule,
    DashboardRoutingModule,
    FlexLayoutModule
  ]
})
export class DashboardModule { }
