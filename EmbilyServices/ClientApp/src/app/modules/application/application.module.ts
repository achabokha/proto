import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TextMaskModule } from 'angular2-text-mask';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';
import { ImageViewerModule } from "ngx-image-viewer";

import { ApplicationRoutingModule } from './application-routing.module';
import { SharedModule } from '../shared/shared.module';

import { AddressComponent } from './address/address.component';
import { PassportComponent } from './passport/passport.component';
import { PaymentComponent } from './payment/payment.component';
import { ShippingAddressComponent } from './shipping-address/shipping-address.component';
import { ProgressIndicatorComponent } from './progress-indicator/progress-indicator.component';
import { SubmitComponent } from './submit/submit.component';
import { TermsComponent } from './terms/terms.component';
import { UploadfileComponent } from './uploadfile/uploadfile.component';

@NgModule({
    imports: [
        CommonModule,
        NgbModule,
        ApplicationRoutingModule,
        SharedModule,
        FormsModule,
        TextMaskModule,
        PdfJsViewerModule,
        ImageViewerModule.forRoot()
    ],
    declarations: [
        AddressComponent,
        PassportComponent,
        PaymentComponent,
        ShippingAddressComponent,
        ProgressIndicatorComponent,
        SubmitComponent,
        TermsComponent,
        UploadfileComponent
    ]
})
export class ApplicationModule { }
