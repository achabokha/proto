import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from '../../services/auth.guard';

import { TermsComponent } from './terms/terms.component';
import { PassportComponent } from './passport/passport.component';
import { AddressComponent } from './address/address.component';
import { ShippingAddressComponent } from './shipping-address/shipping-address.component';
import { PaymentComponent } from './payment/payment.component';
import { SubmitComponent } from './submit/submit.component';
import { ProcessingComponent } from '../shared/processing/processing.component';
import { SignUpComponent } from '../profile/sign-up/sign-up.component';
import { ProductsComponent } from '../shared/products/products.component';


const routes: Routes = [
    {
        path: "",
        //component: ProductsComponent,
        children: [
            { path: '', component: ProductsComponent },
            { path: ':currencyCode/terms/:appId', component: TermsComponent, canActivate: [AuthGuard] },
            { path: ':currencyCode/passport/:appId', component: PassportComponent, canActivate: [AuthGuard] },
            { path: ':currencyCode/address/:appId', component: AddressComponent, canActivate: [AuthGuard] },
            { path: ':currencyCode/shipping-address/:appId', component: ShippingAddressComponent, canActivate: [AuthGuard] },
            { path: ':currencyCode/payment/:appId', component: PaymentComponent, canActivate: [AuthGuard] },
            { path: ':currencyCode/submit/:appId', component: SubmitComponent, canActivate: [AuthGuard] },
            {}

        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class ApplicationRoutingModule {
}
