import { Component, OnDestroy, OnInit, Input } from '@angular/core';
import { TickerService } from 'src/app/services/ticker.service';

@Component({
    selector: 'app-load-limits-live',
    templateUrl: './load-limits-live.component.html',
    styleUrls: ['./load-limits-live.component.css']
})
export class LoadLimitsLiveComponent implements OnInit, OnDestroy {

    @Input() accountCurrencyCode: string | undefined;
    @Input() currencyCode: string | undefined;
    @Input() isKYC: boolean | undefined;

    tickers: any;

    constructor(private tickerService: TickerService) {
    }

    ngOnInit() {
        this.tickerService.getTickers().subscribe((tix) => {
            this.tickers = tix;
        });
    }

    ngOnDestroy() {
    }
}
