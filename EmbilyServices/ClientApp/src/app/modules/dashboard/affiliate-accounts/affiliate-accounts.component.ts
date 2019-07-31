import { Component, Input, ViewChild, Output, EventEmitter } from '@angular/core';
import { AffiliateService } from '../../../services/affiliate.service';
import { DataService } from '../../../services/data.service';
import { Account } from "../../../models";
import { ProcessingComponent } from '../../shared/processing/processing.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-affiliate-accounts',
    templateUrl: './affiliate-accounts.component.html',
    styleUrls: ['./affiliate-accounts.component.css']
})
export class AffiliateAccountsComponent {
    //imgAffiliate = require("../../assets/images/embily-affiliate.png");

    @Input() accounts: Account[] | undefined;
    @Output() refresh = new EventEmitter<void>();

    @ViewChild('redeemModal') redeemModal: any;
    @ViewChild('processing') processing: ProcessingComponent | undefined;

    displayModal: string = 'none';
    redeemFromAccountId: string | undefined;
    redeemFromAccountNumber: string | undefined;
    redeemFromAccountName: string | undefined;
    redeemToAccountId: string | undefined;
    redeemToAccountNumber: string | undefined;
    redeemCurrencyCode: string | undefined;
    redeemBalance: string | undefined;
    complete: boolean = false;
    disableRedeemButton: boolean = true;

    limit = 10;

    constructor(private modalService: NgbModal, private affiliateService: AffiliateService, private data: DataService) {
    }

    ngOnInit(): void {

    }

    showModal(content, currencyCode: string, accountId: string, accountNumber: string, accountName: string, balance: number) {
        if (this.processing) this.processing.state = 'none';

        this.redeemBalance = balance.toFixed(2);
        this.redeemCurrencyCode = currencyCode;
        this.redeemFromAccountId = accountId;
        this.redeemFromAccountNumber = accountNumber;
        this.redeemFromAccountName = accountName;


        if (this.data.accounts)
            if (this.data.accounts.length > 0) {
                let acct = this.data.accounts.find(a => a.currencyCodeString == currencyCode);
                if (acct) {
                    this.redeemToAccountId = acct.accountId;
                    this.redeemToAccountNumber = this.getFormattedCardNumber(acct.providerAccountNumber);
                    this.disableRedeemButton = false;
                }
                else {
                    this.redeemToAccountNumber = 'no suitable account found';
                    this.disableRedeemButton = true;
                }
            }
        this.modalService.open(content);
    }

    redeem() {
        if (this.redeemFromAccountId && this.redeemToAccountId) {
            this.disableRedeemButton = true;
            this.affiliateService.redeemAffliateBalance(this.redeemFromAccountId, this.redeemToAccountId).subscribe(() => {
                this.complete = true;
                this.refresh.emit();
                // TODO: data refresh event --
            }, error => this.disableRedeemButton = false);
        }

    }

    getFormattedCardNumber(number: string) {
        return `${number.slice(0, 4)} ${number.slice(4, 8)} ${number.slice(8, 12)} ${number.slice(12, 16)}`;
    }
}
