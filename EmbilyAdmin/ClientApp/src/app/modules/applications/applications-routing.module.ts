import { NgModule } from '@angular/core';
import { RouterModule, Routes } from "@angular/router";
import { ApplicationsComponent } from './applications.component';
import { ApplicationDetailsComponent } from './application-details/application-details.component';
import { AuthGuard } from '../../services/auth.guard';
import { GeneralComponent } from './general/general.component';
import { PassportComponent } from './passport/passport.component';
import { ShippingAddressComponent } from './shipping-address/shipping-address.component';
import { AddressComponent } from './address/address.component';
import { CardOrdersComponent } from './card-orders/card-orders.component';
import { IntegrationComponent } from './integration/integration.component';

const routes: Routes = [
    {
        path: "",
        //component: ApplicationsComponent,
        children: [
            { path: '', component: ApplicationsComponent },
            { path: ':appId', component: ApplicationDetailsComponent, canActivate: [AuthGuard] },
            { path: 'general/:appId', component: GeneralComponent, canActivate: [AuthGuard] },
            { path: 'passport/:appId', component: PassportComponent, canActivate: [AuthGuard] },
            { path: 'address/:appId', component: AddressComponent, canActivate: [AuthGuard] },
            { path: 'shippingaddress/:appId', component: ShippingAddressComponent, canActivate: [AuthGuard] },
            { path: 'cardOrders/:appId', component: CardOrdersComponent, canActivate: [AuthGuard] },
            { path: 'integration/:appId', component: IntegrationComponent, canActivate: [AuthGuard] },
        ]
    }
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forChild(routes)]
})
export class ApplicationsRoutingModule {
}
