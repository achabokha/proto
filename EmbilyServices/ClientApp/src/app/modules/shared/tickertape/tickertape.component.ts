import { Component, OnInit, OnDestroy } from '@angular/core';
import { TickerService } from 'src/app/services/ticker.service';

@Component({
    selector: 'app-tickertape',
    templateUrl: './tickertape.component.html',
    styleUrls: ['./tickertape.component.scss']
})
export class TickertapeComponent implements OnInit, OnDestroy {

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
