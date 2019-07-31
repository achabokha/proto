import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SpinnerRectComponent } from './spinner-rect/spinner-rect.component';
import { SpinnerRoundComponent } from './spinner-round/spinner-round.component';
import { ProductsComponent } from './products/products.component';
import { FormsModule } from '@angular/forms';
import { TickertapeComponent } from './tickertape/tickertape.component';
import { TopNotificationComponent } from './top-notification/top-notification.component';


@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        FormsModule,
        NgbModule,
    ],
    declarations: [
        SpinnerRectComponent,
        SpinnerRoundComponent,
        ProductsComponent,
        TickertapeComponent,
        TopNotificationComponent,
    ],
    exports: [
        SpinnerRectComponent,
        SpinnerRoundComponent,
        ProductsComponent,
        TickertapeComponent,
        TopNotificationComponent
    ]
})
export class SharedModule { }
