import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountsService } from '../../../services/accounts.service';
import { CryptoAddressComponent } from '../../shared/crypto-address/crypto-address.component';


@Component({
    selector: 'app-load',
    templateUrl: './load.component.html',
    styleUrls: ['./load.component.css']
})
export class LoadComponent implements OnInit {
    displayStateDebug: boolean = false;

    state: string = "loading";
    amount: number | undefined; // amount detected in the Blockchain

    qrCodeSrc: string | undefined;
    address: string | undefined;

    accountId: string | null | undefined;
    currencyCode: string | null | undefined;
    accountCurrencyCode: string | null | undefined;
    isKYC: boolean = true;

    @ViewChild("cryptoAddress") cryptoAddress: CryptoAddressComponent | undefined;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private accountsService: AccountsService,
    ) { }

    // TODO: re-factor: move code to a service --
    ngOnInit(): void {
        this.getData();
    }

    getData() {

        this.accountId = this.route.snapshot.paramMap.get('accountId');
        this.currencyCode = this.route.snapshot.paramMap.get('currencyCode');

        if (this.accountId != null && this.currencyCode != null) {

            this.accountsService.getNewAddress(this.currencyCode, this.accountId).subscribe((r: any) => {
                this.qrCodeSrc = 'data:image/jpeg;base64,' + r.qrCodeImgSrc;
                this.address = r.address;
                this.accountCurrencyCode = r.accountCurrencyCode;
                this.isKYC = r.isKYC;
                this.state = "address";
            }, (error: any) => {
                this.state = "error";
            });

        } else {
            this.state = "error";
        }
    }

    ngOnDestroy() {
    }

    setStateDebug(state: string) {
        if (state == 'transaction') {
            this.amount = 0.12;
            this.state = "transaction";
        } else {
            this.state = state;
        }
    }

    onStatusChange(status: string) {
        switch (status) {
            case 'cancel':
                this.router.navigate(["/dashboard"]);
                break;
            case 'transaction':
                this.state = 'transaction';
                this.amount = (this.cryptoAddress)? this.cryptoAddress.amount : 0;
                break;
            case 'timeout':
                this.state = 'timeout';
                break;
            case 'error':
                this.state = 'error';
                break;
        }
    }
}
