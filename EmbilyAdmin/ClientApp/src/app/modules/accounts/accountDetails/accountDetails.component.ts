import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Account } from "../../../models/account";
import { ActivatedRoute } from '@angular/router';
import { AccountsService } from '../../../services/accounts.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-account-details',
    templateUrl: './accountDetails.component.html',
    styleUrls: ['./accountDetails.component.css']
})

export class AccountDetailsComponent implements OnInit {

    accountId: string;
    accountDetails: Account;

    isBTC: any;
    isLTC: any;
    isETH: any;

    working: boolean = false;

    loadingIndicator: boolean = false;

    transactionStatus: string[] = [];

    amountToLoad: number;
    confirm: boolean = false;
    confirmButton: boolean = false;
    load: boolean = true;

    constructor(private route: ActivatedRoute,
        public accountService: AccountsService,
        public modalService: NgbModal) {
    }

    ngOnInit(): void {
        this.accountId = this.route.snapshot.paramMap.get('accountId');
        this.getData();
    }

    getData() {
        this.accountService.getAccountDetails(this.accountId).subscribe(result => {
            this.accountDetails = result as Account;
            this.isCryptoAddresses();
        });
    }

    isCryptoAddresses() { 
        this.isBTC = this.accountDetails.cryptoAddreses.find(a => a.currencyCodeString == 'BTC');
        this.isLTC = this.accountDetails.cryptoAddreses.find(a => a.currencyCodeString == 'LTC');
        this.isETH = this.accountDetails.cryptoAddreses.find(a => a.currencyCodeString == 'ETH');
    }

    getNewCryptoAddress(currencyCode: string) {
        this.working = true;
        this.accountService.getNewCryptoAddress(this.accountId, currencyCode).subscribe(result => {
            this.working = false;
            this.getData();
        });
    }

    createStatusList() {
    }

    selectStatus(event: any): void {
        this.accountService.setAccountStatus(this.accountId, event.target.value).subscribe(result => {
        })
    }

    update() {
        this.accountService.updateAccount(this.accountDetails).subscribe(result => {

        });
    }

    loadCard() {
        this.confirm = true;
        this.confirmButton = true;
        this.load = false;
    }

    confirmAmount() {
        this.confirmButton = false;
        this.accountService.loadAmount({ accountId: this.accountDetails.accountId, amount: this.amountToLoad })
        .subscribe(r => {

        }, error => {

        });     
    }

    closeModal() {

        this.confirm = false;
        this.confirmButton = false;
        this.amountToLoad = null;
        this.load = true;

        this.modalService.dismissAll();
    }
}
