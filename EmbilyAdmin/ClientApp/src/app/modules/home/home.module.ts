import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { DataService } from '../../services/data.service';
import { NavbarService } from '../../services/navbar.service';
import { AuthService } from '../../services/auth.service';
import { AuthGuard } from '../../services/auth.guard';

import { SharedModule } from '../shared/shared.module';
import { LayoutComponent } from './layout/layout.component';
import { HeaderComponent } from './header/header.component';
import { FooterComponent } from './footer/footer.component';
import { LoadingComponent } from './loading/loading.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SidebarComponent } from './sidebar/sidebar.component';
import { ProfileModule } from '../profile/profile.module';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        SharedModule,
        ProfileModule,
        NgbModule,
        TranslateModule
    ],
    declarations: [
        LayoutComponent,
        HeaderComponent,
        FooterComponent,
        LoadingComponent,
        SidebarComponent
    ],
    exports: [LayoutComponent],
    providers: [
        DataService,
        NavbarService,
        AuthService,
        AuthGuard
    ]
})
export class HomeModule { }
