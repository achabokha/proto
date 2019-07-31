import { Component, OnInit, Inject, Input, Output, OnDestroy, EventEmitter } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { trigger, state, style, animate, transition } from '@angular/animations';
import { Subscription, Observable, timer} from 'rxjs';

import { BlockchainService } from "../../../services/blockchain.service";
import { BitcoinService } from "../../../services/bitcoin.service";
import { EthereumService } from "../../../services/ethereum.service";
import { LitecoinService } from "../../../services/litecoin.service";

@Component({
    selector: 'app-crypto-address',
    templateUrl: './crypto-address.component.html',
    styleUrls: ['./crypto-address.component.css'],
    animations: [
        trigger('flyInOut', [
            state('in', style({ opacity: 1, transform: 'translateY(0)' })),
            transition('void => *', [
                style({ opacity: 0, transform: 'translateY(20%)' }),
                animate("200ms 300ms ease-in")
            ]),
            transition('* => void', [
                animate('200ms 200ms ease-out',
                    style({ opacity: 0, transform: 'translateY(-20%)' }))
            ])
        ])
    ]
})
export class CryptoAddressComponent implements OnInit, OnDestroy {

    isDebug: boolean;
    images: any = {
        'BTC': 'assets/images/currencies/btc.png',
        'LTC': 'assets/images/currencies/ltc.png',
        'ETH': 'assets/images/currencies/eth.png'
    };

    coinImage: any;
    coinName: string | undefined;

    isCopied1: boolean = false;
    flyInOut: string = "in";

    @Input() currencyCode: string = "";
    @Input() address: string = "";
    @Input() qrCodeSrc: string = "";
    @Output() statusChange = new EventEmitter<string>();


    minutesDisplay: number = 0;
    secondsDisplay: number = 0;

    state: string = 'address';
    amount: number = 0;

    private blockchainService: BlockchainService;
    private chainSubscription: Subscription;
    private timerSubscription: Subscription;

    seconds: number = 60 * 20;

    constructor(
        private bitcoinService: BitcoinService,
        private ethereumService: EthereumService,
        private litecoinService: LitecoinService,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {

        this.isDebug = this.route.snapshot.queryParamMap.get('debug') == 'true';

        console.log("CryptoAddress::ngOnInit() ...")

        this.coinImage = this.images[this.currencyCode];

        this.blockchainService = this.getBlockchainService(this.currencyCode);

        this.coinName = this.blockchainService.getCoinName();

        this.chainSubscription = this.blockchainService.amountDetected
            .subscribe((amount: number) => {
                this.updateTransactionState(amount);
            }, (error: any) => {
                console.error(error);
                this.statusChange.emit('error');
            });

        this.blockchainService.connectAndMonitor(this.address);

        this.startTimer();
    }

    ngOnDestroy() {
        this.clean();

        console.log("CryptoAddress::ngOnDestroy() ...")
    }

    cancel() {
        this.statusChange.emit('cancel');
    }

    private startTimer() {
        this.timerSubscription = timer(1, 1000).subscribe(t => {
            let ticks = this.seconds - t;

            if (ticks < 0) {
                this.timerSubscription.unsubscribe();
                this.statusChange.emit('timeout');
            }

            this.secondsDisplay = this.getSeconds(ticks);
            this.minutesDisplay = this.getMinutes(ticks);
        });
    }

    // TODO: should go to BlockchainService base class, this is a factory method --
    getBlockchainService(currencyCode: string): BlockchainService {
        switch (currencyCode) {
            case 'BTC':
                return this.bitcoinService;
            case 'ETH':
                return this.ethereumService;
            case 'LTC':
                return this.litecoinService;
            default:
                throw "unsupported currency";
        }
    }

    updateTransactionState(amount: number) {
        this.amount = amount;
        this.statusChange.emit('transaction');
    }

    clean() {
        if (this.blockchainService && this.blockchainService.connected) this.blockchainService.close();
        if (this.chainSubscription) this.chainSubscription.unsubscribe();
        if (this.timerSubscription) this.timerSubscription.unsubscribe();
    }

    copyToClipboard(): void {
        // Create a "hidden" input
        var aux = document.createElement("input");

        // Assign it the value of the specified element
        aux.setAttribute("value", this.address);

        // Append it to the body
        document.body.appendChild(aux);

        // Highlight its content
        aux.select();

        // Copy the highlighted text
        document.execCommand("copy");

        // Remove it from the body
        document.body.removeChild(aux);

        this.isCopied1 = true;
    }

    private getSeconds(ticks: number) {
        return this.pad(ticks % 60);
    }

    private getMinutes(ticks: number) {
        return this.pad((Math.floor(ticks / 60)) % 60);
    }

    private pad(digit: any) {
        return digit <= 9 ? '0' + digit : digit;
    }


    // ------ debugging ------ 
    debugTransaction() {
        this.updateTransactionState(0.21);
    }

    debugTimeout() {
        this.statusChange.emit('timeout');
    }
    
    debugError() {
        this.statusChange.emit('error');
    }
}
