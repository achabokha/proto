import { NgModule, InjectionToken } from "@angular/core";
import { CommonModule } from "@angular/common";

import { LoginRoutingModule } from "./login-routing.module";
import { FormsModule } from "@angular/forms";
import { LoginComponent } from "./login.component";
import { FlexLayoutModule } from "@angular/flex-layout";
import { SignUpComponent } from "./sign-up/sign-up.component";
import { ReactiveFormsModule } from "@angular/forms";
import { AuthComponent } from "./auth-ui/auth-ui.component";
import { NgxAuthFirebaseUIConfig, ngxAuthFirebaseUIConfigFactory } from "../interfaces/config.interface";

export const UserProvidedConfigToken = new InjectionToken<NgxAuthFirebaseUIConfig>("UserProvidedConfigToken");
import { MatPasswordStrengthModule } from "@angular-material-extensions/password-strength";
import { EmailConfirmationComponent } from "./email-confirmation/email-confirmation.component";
import { NgxAuthFirebaseUIConfigToken } from "../services/auth-process.service";
import { AuthProvidersComponent } from "./auth-provider/auth.providers.component";
import { UserComponent } from "./user/user.component";
import { MaterialModule } from '../material-modules';

@NgModule({
    declarations: [
        LoginComponent,
        SignUpComponent,
        AuthComponent,
        EmailConfirmationComponent,
        AuthProvidersComponent,
        UserComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        FlexLayoutModule,
        LoginRoutingModule,
        MaterialModule,
        MatPasswordStrengthModule
    ],
    providers: [
        {
            provide: NgxAuthFirebaseUIConfigToken,
            useFactory: ngxAuthFirebaseUIConfigFactory,
            deps: [UserProvidedConfigToken]
        }
    ]
})
export class LoginModule {}
