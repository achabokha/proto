import { Address } from './../../../models/address';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

import { CheckoutBase } from './../common/checkout-base';
import { Countries } from '../../../models/countries';
import { ApplicationService } from '../../../services/application.service';

@Component({
    selector: 'app-shipping-address',
    templateUrl: './shipping-address.component.html',
    styleUrls: ['./shipping-address.component.css']
})
export class ShippingAddressComponent extends CheckoutBase implements OnInit {

    shipping = 'assets/images/checkout/shipping.png';

    shippingAddress: Address = new Address();
    sameAsAddress: any; 

    countries: any[] = Countries.list;
    country: any;

    shippingAddressId: string = "";

    shippingDetail: any;
    selectedShippingOption: number = -1;
    spinner: boolean = false;
    shippingOptionDisabled: boolean = false;

    constructor(private router: Router, route: ActivatedRoute, private applicationService: ApplicationService) {
        super(route);
    }

    ngOnInit() {
        this.shippingOptionDisabled = this.queryParams && this.queryParams.resubmit;
        this.applicationService.getShippingAddressAndShippingOptions(this.appId).subscribe((result) => {
            this.shippingAddress = result.address;
            this.selectedShippingOption = result.selectedShippingOption;
            this.shippingDetail = result.shippingDetail;
            if (this.selectedShippingOption == -1) this.shippingOptionDisabled = false;
        });
    }

    onChangeAddress(event: any) {
        if (event.target.checked) {
            this.applicationService.getAddress(this.appId).subscribe((address) => {
                this.shippingAddressId = this.shippingAddress.addressId;
                this.shippingAddress = address;
                this.shippingAddress.addressId = this.shippingAddressId;
                this.getShippingOptions(event);
            });
        }
    }

    continue() {
        this.applicationService.updateShippingAddressAndShippingOptions(
            this.appId, this.shippingAddress, this.shippingDetail, this.selectedShippingOption)
            .subscribe(() => {
                let navigate = this.queryParams && this.queryParams.resubmit ? "submit" : "payment";
                this.router.navigate(["application", this.currencyCode, navigate, this.appId], { queryParams: this.queryParams });
            });
    }

    back() {
        this.router.navigate(["application", this.currencyCode, "address", this.appId], { queryParams: this.queryParams });
    }

    getShippingOptions(value: any) {
        //console.log(this.shippingAddress.postalCode);
        //console.log(this.shippingAddress.country);
        //console.log(this.currencyCode);

        // call shipping options      
        if (this.shippingAddress.postalCode && this.shippingAddress.country && this.currencyCode) {
            this.country = this.countries.find(c => c.iso3 == this.shippingAddress.country);
            this.selectedShippingOption = -1;
            this.spinner = true;
            this.applicationService.getShippingOptions(
                this.appId, this.currencyCode, this.shippingAddress.postalCode, this.country.iso2)
                //.finally(() => this.spinner = false)
                .subscribe((result) => {
                    this.shippingDetail = result.options;
                    this.spinner = false;
                });
        }
    }
}
