import { Injectable } from '@angular/core';
import { BlockchainService } from "./blockchain.service";

@Injectable()
export class BitcoinService extends BlockchainService {

    protected coinName: string = "Bitcoin";
    protected url: string = "wss://ws.blockchain.info/inv";

    monitor() {
        if (this.socket) {
            let msg = JSON.stringify({ "op": "addr_sub", "addr": this.address });
            this.socket.send(msg);

            console.log(" Monitoring BTC address", this.address);
        }
    }

    processMessage(msgString: any) {
        if (typeof WebSocket !== 'undefined') {
            let msg = JSON.parse(msgString);
            let r = msg.x.out.find((o: any) => o.addr == this.address);
            let amount = +(+r.value / 100000000).toFixed(8);
            let input_txn_hash = msg.x.hash;

            console.log(
                " BTC address:", this.address,
                " BTC txn hash:", input_txn_hash,
                " Amount detected: ", amount,
                " Response: " + JSON.stringify(msg)
            );

            this.amountDetected.next(amount);
        }
    }

    close(): void {
        if (this.socket) {
            let msg = JSON.stringify({ "op": "addr_unsub", "addr": this.address });
            this.socket.send(msg);
            console.log(" BTC sends close msg")
        }
        super.close();
    }

}
