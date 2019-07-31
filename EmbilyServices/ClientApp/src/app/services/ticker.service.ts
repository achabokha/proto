import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable()
export class TickerService {

    public tickers: any = {
        BTCUSD: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 },
        ETHUSD: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 },
        LTCUSD: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 },
        BTCEUR: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 },
        ETHEUR: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 },
        LTCEUR: { chanId: 0, price: 0, dailyChange: 0, dailyChangePercent: 0 },
    };

    wss: any;

    constructor() { }

    getTickers(): Observable<any> {
        if (typeof WebSocket !== 'undefined' && typeof this.wss == 'undefined') {
            this.initTicker();
        }
        return of(this.tickers);
    }

    initTicker() {

        this.wss = new WebSocket('wss://api.bitfinex.com/ws/2')

        this.wss.onmessage = (response: any) => {
            var data = JSON.parse(response.data);
            this.processData(data);
        }

        this.wss.onopen = () => {
            // subscribe for ticker 
            // docs: https://docs.bitfinex.com/v1/reference#ws-public-ticker
            var msg_s_btcusd = { event: "subscribe", channel: "ticker", pair: "BTCUSD" };
            var msg_s_ethusd = { event: "subscribe", channel: "ticker", pair: "ETHUSD" };
            var msg_s_ltcusd = { event: "subscribe", channel: "ticker", pair: "LTCUSD" };
            var msg_s_btceur = { event: "subscribe", channel: "ticker", pair: "BTCEUR" };
            var msg_s_etheur = { event: "subscribe", channel: "ticker", pair: "ETHEUR" };
            var msg_s_ltceur = { event: "subscribe", channel: "ticker", pair: "LTCEUR" };

            this.wss.send(JSON.stringify(msg_s_btcusd));
            this.wss.send(JSON.stringify(msg_s_ethusd));
            this.wss.send(JSON.stringify(msg_s_ltcusd));
            this.wss.send(JSON.stringify(msg_s_btceur));
            this.wss.send(JSON.stringify(msg_s_etheur));
            this.wss.send(JSON.stringify(msg_s_ltceur));
        }
    }

    processData(data: any) {
        // if data is array when it is a ticker info, if data is an object then it is a  response to subscription to a particular ticker pair,
        // from that object we retrieve channel id. Channel id is basically the pair (i.e.: BTCUSD),
        // there is also Heart beat object is being sent sometimes.
        // no comments -- 
        if (data instanceof Array) {
            if (data[1] instanceof Array) {
                this.processTicker(data[0], data[1])
            } else if (data[1] === "hb") {
                // heart beat --
                //console.log("heart beat!")
            }
        }
        else {
            // match channelId with ticker pair for initial setup --
            if (typeof data.event != "undefined" && data.event == "subscribed" || data.event == "info") {
                if (data.event == "subscribed" && typeof data.chanId != "undefined" && typeof data.pair != "undefined") {

                    this.tickers[data.pair].chanId = data.chanId;
                }
            }
        }
    }

    processTicker(chanId: any, ticker: any) {
        // 1) find the pair (i.e.: BTCUSD) by channel id --
        let t_key = Object.keys(this.tickers).find(key => this.tickers[key].chanId == chanId);

        /*  ticker Array values: 
            BID	                float	Price of last highest bid
            BID_SIZE	        float	Size of the last highest bid
            ASK	                float	Price of last lowest ask
            ASK_SIZE	        float	Size of the last lowest ask
            DAILY_CHANGE	    float	Amount that the last price has changed since yesterday
            DAILY_CHANGE_PERC	float	Amount that the price has changed expressed in percentage terms
            LAST_PRICE	        float	Price of the last trade. (that's the price!--)
            VOLUME	            float	Daily volume
            HIGH        	    float	Daily high
            LOW	                float	Daily low                    
        */

        // 2) update our ticker -- 
        if (t_key) {

            this.tickers[t_key].dailyChange = ticker[4]
            this.tickers[t_key].dailyChangePercent = ticker[5]
            this.tickers[t_key].price = ticker[6]

            //console.log(this.tickers);
        }
    }
}
