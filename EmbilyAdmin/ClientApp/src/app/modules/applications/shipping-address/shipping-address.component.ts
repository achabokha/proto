import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { ApplicationService } from '../../../services/application.service';

@Component({
    selector: 'app-app-shipping-address',
    templateUrl: './shipping-address.component.html',
    styleUrls: ['./shipping-address.component.css']
})
export class ShippingAddressComponent implements OnInit {

    shippingOptions: any;
    selectedShippingOption: number = -1;
    shippingOptionDisabled: boolean = true;

    constructor(public data: DataService, private applicationService: ApplicationService) {
    }

    ngOnInit(): void {
        if (this.data.selectApp.shippingOptions) {
            this.shippingOptions = JSON.parse(this.data.selectApp.shippingOptions);
            this.selectedShippingOption = this.data.selectApp.selectedShippingOption; 
        }
    }

    updateAddress() {
        this.applicationService.updateAddress(this.data.shippingAddress).subscribe(result => {

        });
    }

    update() {
        this.applicationService.updateApplicationInfo(this.data.selectApp).subscribe(result => {

        });
    }
}
