import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";

import { UploadfileComponent } from "../uploadfile/uploadfile.component";
import { CheckoutBase } from '../common/checkout-base';
import { ApplicationService } from '../../../services/application.service';
import { Countries } from '../../../models/countries';
import { Address } from '../../../models/address';
import { PassportInfo } from '../../../models';

import { DataService } from '../../../services/data.service';

@Component({
    selector: 'app-address',
    templateUrl: './address.component.html',
    styleUrls: ['./address.component.css'] 
})
export class AddressComponent extends CheckoutBase implements OnInit {

    @ViewChild('uploadFile1') uploadFile1: UploadfileComponent | any;

    addressExample = 'assets/images/checkout/address-example.png';

    address: Address = new Address();
    countries: any[] = Countries.list;

    constructor(private router: Router, route: ActivatedRoute, private applicationService: ApplicationService) {
        super(route);
    }

    ngOnInit(): void {
        this.applicationService.getDocumentInfo(this.appId, 'ProofOfAddress').subscribe((docInfo) => {
            this.uploadFile1.docInfo = docInfo;
            this.uploadFile1.continueDisabled = docInfo.fileType == 'none';
            this.uploadFile1.fileLabel = docInfo.fileType == 'none' ? "Choose a file:" : "Choose another file:";
        });
        this.applicationService.getAddress(this.appId).subscribe((address) => {
            this.address = address;
        });
    }

    ngOnDestroy() {
    }

    back() {
        this.router.navigate(["application", this.currencyCode, "passport", this.appId], { queryParams: this.queryParams });
    }

    continue() {
        this.applicationService.updateAddress(this.address).subscribe(() => {
            this.router.navigate(["application", this.currencyCode, "shipping-address", this.appId], { queryParams: this.queryParams })
        });
    }
}
