import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { CustomMaterialModuleModule } from '../custom-material-module/custom-material-module.module';
import { IonicModule } from '@ionic/angular';


@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    IonicModule,
    CustomMaterialModuleModule,
    DashboardRoutingModule
  ]
})
export class DashboardModule { }
