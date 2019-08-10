import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { IonicModule } from '@ionic/angular';
import { CustomMaterialModuleModule } from '../custom-material-module/custom-material-module.module';
import { FormsModule } from '@angular/forms';
import { LoginComponent } from './login.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { SignUpComponent } from './sign-up/sign-up.component';



@NgModule({
  declarations: [ LoginComponent, SignUpComponent ],
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    CustomMaterialModuleModule,
    FlexLayoutModule,
    LoginRoutingModule
  ]
})
export class LoginModule { }
