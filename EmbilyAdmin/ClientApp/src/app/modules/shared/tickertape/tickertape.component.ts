import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
    selector: 'app-tickertape',
    templateUrl: './tickertape.component.html',
    styleUrls: ['./tickertape.component.scss']
})
export class TickertapeComponent implements OnInit, OnDestroy {

    tickers: any = {
        BTCUSD: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 },
        ETHUSD: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 },
        LTCUSD: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 }
    };

    wss: any;

    constructor() {
        this.initTicker();
    }

    ngOnInit() {
    }

    ngOnDestroy() {
        //if (this.wss) this.wss.close();
    }

    initTicker() {
        // WebSocket exception was happannig here, only --
        if (typeof WebSocket !== 'undefined') {
            this.wss = new WebSocket('wss://api.bitfinex.com/ws/2')
            this.wss.onopen = () => {
                // subscribe for ticker
                // docs: https://docs.bitfinex.com/v1/reference#ws-public-ticker
                var msg_s_btcusd = { event: "subscribe", channel: "ticker", pair: "BTCUSD" };
                var msg_s_ethusd = { event: "subscribe", channel: "ticker", pair: "ETHUSD" };
                var msg_s_ltcusd = { event: "subscribe", channel: "ticker", pair: "LTCUSD" };

                this.wss.send(JSON.stringify(msg_s_btcusd));
                this.wss.send(JSON.stringify(msg_s_ethusd));
                this.wss.send(JSON.stringify(msg_s_ltcusd));

            }
            this.wss.onmessage = (response: any) => {
                var data = JSON.parse(response.data);
                this.processData(data);
            }
        }
    }

    processData(data: any) {
        if (data instanceof Array) {
            if (data[1] instanceof Array) {
                this.processTicker(data[0], data[1])
            } else if (data[1] === "hb") {
                // heart beat --
                //console.log("heart beat!")
            }
        }
        else {
            if (typeof data.event != "undefined" && data.event == "subscribed" || data.event == "info") {
                if (data.event == "subscribed" && typeof data.chanId != "undefined" && typeof data.pair != "undefined") {
                    // match channelId with ticker pair
                    this.tickers[data.pair].chanId = data.chanId;
                }
            }
        }
    }

    processTicker(chanId: any, ticker: any) {
        let t_key = Object.keys(this.tickers).find(key => this.tickers[key].chanId == chanId);

        /*  ticker Array values: 
            BID	                float	Price of last highest bid
            BID_SIZE	        float	Size of the last highest bid
            ASK	                float	    Price of last lowest ask
            ASK_SIZE	        float	Size of the last lowest ask
            DAILY_CHANGE	    float	Amount that the last price has changed since yesterday
            DAILY_CHANGE_PERC	float	Amount that the price has changed expressed in percentage terms
            LAST_PRICE	        float	Price of the last trade.
            VOLUME	            float	Daily volume
            HIGH        	    float	Daily high
            LOW	                float	Daily low                    
        */

        if (t_key) {

            this.tickers[t_key].dailyChange = ticker[4]
            this.tickers[t_key].dailyChangePercent = ticker[5]
            this.tickers[t_key].price = ticker[6]

            //console.log(this.tickers);
        }
    }
}
