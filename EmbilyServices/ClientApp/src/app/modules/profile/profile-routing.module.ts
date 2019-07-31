import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { SettingsComponent } from './settings/settings.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { AuthGuard } from '../../services/auth.guard';
import { SignUpComponent } from './sign-up/sign-up.component';
import { ConfirmEmailComponent } from './confirm-email/confirm-email.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { LoginComponent } from './login/login.component';
import { ReportLostCardComponent } from './report-lost-card/report-lost-card.component';
import { SetpinComponent } from './setpin/setpin.component';
import { SetpinParentComponent } from './setpin-parent/setpin-parent.component';
 

const routes: Routes = [
    {
        path: "",
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'signup/:token', component: SignUpComponent },
            { path: 'signup', component: SignUpComponent },
            { path: 'confirmemail', component: ConfirmEmailComponent },
            { path: 'forgotpassword', component: ForgotPasswordComponent },
            { path: 'resetpassword', component: ResetPasswordComponent },

            { path: "general", component: SettingsComponent, canActivate: [AuthGuard] },
            { path: "change-password", component: ChangePasswordComponent, canActivate: [AuthGuard] },
            { path: 'lostcard', component: ReportLostCardComponent, canActivate: [AuthGuard] },
            { path: 'setcardpin', component: SetpinParentComponent, canActivate: [AuthGuard] },
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class ProfileRoutingModule {
}
