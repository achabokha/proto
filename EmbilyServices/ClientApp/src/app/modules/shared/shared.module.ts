import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { SpinnerRectComponent } from './spinner-rect/spinner-rect.component';
import { SpinnerRoundComponent } from './spinner-round/spinner-round.component';
import { ProcessingComponent } from './processing/processing.component';
import { ProductsComponent } from './products/products.component';
import { CryptoAddressComponent } from './crypto-address/crypto-address.component';
import { FormsModule } from '@angular/forms';
import { TickertapeComponent } from './tickertape/tickertape.component';


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
        ProcessingComponent,
        ProductsComponent,
        CryptoAddressComponent,
        TickertapeComponent
    ],
    exports: [
        SpinnerRectComponent,
        SpinnerRoundComponent,
        ProcessingComponent,
        ProductsComponent,
        CryptoAddressComponent,
        TickertapeComponent
    ]
})
export class SharedModule { }
