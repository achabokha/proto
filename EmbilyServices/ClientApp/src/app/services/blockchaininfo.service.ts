import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable()
export class BlockchaininfoService {

    info: any = {
        txnId: "",
        amount: 0,
        addr: ""
    };

    socket: any;

    constructor() { }

    getBlockchainInfo(): Observable<any> {
        if (typeof WebSocket !== 'undefined' && typeof this.socket == 'undefined') {
            this.init();
        }
        return of(this.info);
    }
    
    init(): void {
        this.socket = new WebSocket("wss://ws.blockchain.info/inv")

        this.socket.onmessage = (r: any) => {
            this.processMessage(r.data);
        };

        this.socket.onerror = () => {
            console.log('error in the socket');
        }

        this.socket.onopen = () => {
            if (this.socket) this.socket.send('{"op":"unconfirmed_sub"}');
        }
    }

    processMessage(message: any) {
        let msg = JSON.parse(message);
        this.info.txnId = msg.x.tx_index;
        this.info.amount = msg.x.out[0].value / 100000000;
        this.info.addr = msg.x.out[0].addr;
    }
}
