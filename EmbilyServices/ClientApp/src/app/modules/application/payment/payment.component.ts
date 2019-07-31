import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { CheckoutBase } from '../common/checkout-base';
import { ApplicationService } from '../../../services/application.service';
import { DataService } from '../../../services/data.service';

@Component({
    selector: 'app-payment',
    templateUrl: './payment.component.html',
    styleUrls: ['./payment.component.css']
})
export class PaymentComponent extends CheckoutBase implements OnInit {

    orderId: string | undefined;
    address: string | undefined;
    qrCodeSrc: string | undefined;
    amount: number | undefined;
    cryptoCurrencyCode: string = '';

    images: any = {
        'BTC': 'assets/images/currencies/btc.png',
        'LTC': 'assets/images/currencies/ltc.png',
        'ETH': 'assets/images/currencies/eth.png'
    };

    complete: boolean = false;
    spinner: boolean = false;

    cardCost: number = 0;
    shippingCost: number = 0;
    total: number = 0;

    state: string = "payment";

    constructor(private router: Router, route: ActivatedRoute, private applicationService: ApplicationService, public data: DataService) {
        super(route);
    }

    ngOnInit(): void {
        this.getData();
    }

    getData() {

        this.applicationService.getOrderCost(this.appId)
            //.finally(() => this.spinner = false)
            .subscribe((result) => {
                this.cardCost = result.cardCost;
                this.shippingCost = result.shippingCost;
                this.total = result.total;
            });
    }

    payment(currencyCode: string) {
        this.state = "spinner";

        // API params 
        this.cryptoCurrencyCode = currencyCode;

        this.applicationService.getOrderDetails(this.appId, this.cryptoCurrencyCode).subscribe((r: any) => {

            this.orderId = r.orderId;
            this.amount = r.amount;
            this.address = r.cryptoAddress;
            this.qrCodeSrc = "data:image/png;base64," + r.qrCodeSrc;
            this.state = 'address';
        });

        // TODO: error processing -- 
    }

    back() {
        this.router.navigate(["application", this.currencyCode, "shipping-address", this.appId], { queryParams: this.queryParams });
    }

    payLater() {
        this.data.stage = null;
        this.router.navigate(["/dashboard"]);
    }

    timeout() {
        this.data.stage = null;
        this.state = 'payment';
    }

    awaitingPayment() {
        if (this.orderId) {

            this.applicationService.awaitingPayment(this.appId, this.orderId).subscribe(
                () => {
                    this.complete = true;
                    this.data.stage = null;
                }, error => {
                    this.state = 'error';
                });
        }
        else {
            console.log('order id is not defined');
        }
    }

    onStatusChange(status: string) {
        switch (status) {
            case 'cancel':
                this.state = 'payment';
                break;
            case 'transaction':
                //this.state = 'transaction';
                this.awaitingPayment();
                break;
            case 'timeout':
                this.state = 'timeout';
                break;
            case 'error':
                this.state = 'error';
                break;
        }
    }

    justSubmit() {
        this.state = "spinner";
        this.applicationService.submit(this.appId)
            .subscribe(() => {
                this.complete = true;
                this.data.clean();
            }, error => {
                this.state = 'error';
            });
    }
}
