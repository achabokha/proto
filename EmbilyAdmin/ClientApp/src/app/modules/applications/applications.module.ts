import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { FormsModule } from '@angular/forms';
import { PdfJsViewerModule } from 'ng2-pdfjs-viewer';
import { ImageViewerModule } from "ngx-image-viewer";
//import { ImageViewerModule } from '@hallysonh/ngx-imageviewer';

import { ApplicationsRoutingModule } from './applications-routing.module';
import { ApplicationsComponent } from './applications.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { ApplicationDetailsComponent } from './application-details/application-details.component';
import { GeneralComponent } from './general/general.component';
import { PassportComponent } from './passport/passport.component';
import { AddressComponent } from './address/address.component';
import { ShippingAddressComponent } from './shipping-address/shipping-address.component';
import { CardOrdersComponent } from './card-orders/card-orders.component';
import { IntegrationComponent } from './integration/integration.component';
import { CommentsComponent } from './comments/comments.component';

@NgModule({
  imports: [
      CommonModule,
      ApplicationsRoutingModule,
      RouterModule,
      NgbModule, // bootstrap helpers
      NgxDatatableModule,
      SharedModule,
      FormsModule,
      PdfJsViewerModule,
      ImageViewerModule.forRoot()
      //ImageViewerModule
  ],
    declarations: [
        ApplicationsComponent,
        ApplicationDetailsComponent,
        GeneralComponent,
        PassportComponent,
        AddressComponent,
        ShippingAddressComponent,
        CardOrdersComponent,
        IntegrationComponent,
        CommentsComponent,
    ]
})
export class ApplicationsModule { }
