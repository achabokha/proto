import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { PassportInfo } from '../../../models';
import { Countries } from '../../../models/countries';
import { ApplicationService } from '../../../services/application.service';
import { DataService } from '../../../services/data.service';

import { UploadfileComponent } from "../uploadfile/uploadfile.component";
import { CheckoutBase } from '../common/checkout-base';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-passport',
    templateUrl: './passport.component.html',
    styleUrls: ['./passport.component.css']
})
export class PassportComponent extends CheckoutBase implements OnInit {

    model: PassportInfo | undefined;

    countries: any[] = Countries.list;

    passportExample = 'assets/images/checkout/passport-example.png';

    @ViewChild('uploadFile1') uploadFile1: UploadfileComponent;
    @ViewChild('uploadFile2') uploadFile2: UploadfileComponent;

    public mask = [/[1-9]/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/];

    //public dateOfBirth: any;

    constructor(private router: Router,
        route: ActivatedRoute,
        private applicationService: ApplicationService,
        private data: DataService,
        config: NgbDatepickerConfig) {

        super(route);
        config.minDate = { year: 1900, month: 1, day: 1 };
    }

    ngOnInit() {
        if (this.appId != 'start') {
            this.applicationService.getPassportInfo(this.appId).subscribe((passportInfo) => {
                this.model = passportInfo;
            });
        }
        this.applicationService.getDocumentInfo(this.appId, 'ProofOfID').subscribe((docInfo) => {
            this.uploadFile1.docInfo = docInfo;
            this.uploadFile1.continueDisabled = docInfo.fileType == 'none';
            this.uploadFile1.fileLabel =  docInfo.fileType == 'none' ? "Choose a file:" : "Choose another file:";
        });
        this.applicationService.getDocumentInfo(this.appId, 'Selfie').subscribe((docInfo) => {
            this.uploadFile2.docInfo = docInfo;
            this.uploadFile2.continueDisabled = docInfo.fileType == 'none';
            this.uploadFile2.fileLabel = docInfo.fileType == 'none' ? "Choose a file:" : "Choose another file:";
        });
    }

    back() {
        this.router.navigate(["application", this.currencyCode, "terms", this.appId], { queryParams: this.queryParams });
    }

    continue() {
        if (this.model) {
            this.applicationService.updatePassportInfo(this.model).subscribe(() => {
                this.router.navigate(["application", this.currencyCode, "address", this.appId], { queryParams: this.queryParams });
            });
        }
    }
}
