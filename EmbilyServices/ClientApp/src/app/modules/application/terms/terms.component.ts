import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { CheckoutBase } from './../common/checkout-base';
import { ApplicationService } from '../../../services/application.service';
import { DataService } from '../../../services/data.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-terms',
    templateUrl: './terms.component.html',
    styleUrls: ['./terms.component.css']
})
export class TermsComponent extends CheckoutBase implements OnInit {

    complete: boolean = false;
    spinner: boolean = false;

    constructor(private modalService: NgbModal, private router: Router, route: ActivatedRoute, private applicationService: ApplicationService, private data: DataService) {
        super(route);
    }

    ngOnInit() {
    }

    continue() {
        if (this.appId == 'start') {
            this.applicationService.startApplication(this.currencyCode).subscribe((appId) => {
                this.appId = appId;
                this.router.navigate(["application", this.currencyCode, "passport", this.appId], { queryParams: this.queryParams });
            });
        }
        else {
            this.router.navigate(["application", this.currencyCode, "passport", this.appId], { queryParams: this.queryParams });
        }
    }

    showModal(content) {
        //if (this.processing) this.processing.state = 'none';

        //this.redeemBalance = balance.toFixed(2);
        //this.redeemCurrencyCode = currencyCode;
        //this.redeemFromAccountId = accountId;
        //this.redeemFromAccountNumber = accountNumber;
        //this.redeemFromAccountName = accountName;


        //if (this.data.accounts)
        //    if (this.data.accounts.length > 0) {
        //        let acct = this.data.accounts.find(a => a.currencyCodeString == currencyCode);
        //        if (acct) {
        //            this.redeemToAccountId = acct.accountId;
        //            this.redeemToAccountNumber = this.getFormattedCardNumber(acct.providerAccountNumber);
        //            this.disableRedeemButton = false;
        //        }
        //        else {
        //            this.redeemToAccountNumber = 'no suitable account found';
        //            this.disableRedeemButton = true;
        //        }
        //    }
        this.modalService.open(content);
    }
}
