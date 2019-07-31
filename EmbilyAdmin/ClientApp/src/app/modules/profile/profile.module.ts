import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { ProfileRoutingModule } from './profile-routing.module';
import { SharedModule } from '../shared/shared.module'; 

import { SettingsComponent } from './settings/settings.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { LoginComponent } from './login/login.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { SignOutComponent } from './sign-out/sign-out.component';
import { SignUpComponent } from './sign-up/sign-up.component';


@NgModule({
    imports: [
        CommonModule,
        NgbModule,
        FormsModule,
        ProfileRoutingModule,
        SharedModule,
    ],
    declarations: [
        SettingsComponent,
        ChangePasswordComponent,
        ConfirmEmailComponent,
        ForgotPasswordComponent,
        LoginComponent,
        ResetPasswordComponent,
        SignOutComponent,
        SignUpComponent,
    ],
    exports: [
        LoginComponent    
    ]
})
export class ProfileModule { }
