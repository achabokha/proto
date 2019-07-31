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
import { ReportLostCardComponent } from './report-lost-card/report-lost-card.component';
import { LostCardComponent } from './lost-card/lost-card.component';

import { RecaptchaModule } from 'ng-recaptcha';
import { RecaptchaFormsModule } from 'ng-recaptcha/forms';
import { SetpinComponent } from './setpin/setpin.component';
import { SetpinParentComponent } from './setpin-parent/setpin-parent.component';


@NgModule({
    imports: [
        CommonModule,
        NgbModule,
        FormsModule,
        ProfileRoutingModule,
        SharedModule,
        RecaptchaModule,
        RecaptchaFormsModule ,
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
        ReportLostCardComponent,
        LostCardComponent,
        SetpinComponent,
        SetpinParentComponent
    ]
})
export class ProfileModule { }
