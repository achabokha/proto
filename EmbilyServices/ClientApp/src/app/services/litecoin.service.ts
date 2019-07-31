import { Injectable } from '@angular/core';
import { BlockchainService } from "./blockchain.service";

@Injectable()
export class LitecoinService extends BlockchainService {

    protected coinName: string = "Litecoin";
    protected url: string = "wss://socket.blockcypher.com/v1/ltc/main";

    protected monitor() {
        if (this.socket) {
            let msg = JSON.stringify({
                event: 'tx-confirmation',
                address: this.address,
                token: '07d6371ae3bd472cac605875b2c38e22'
            });

            this.socket.send(msg);

            console.log('Monitoring message sent: ', msg)
        }
    }

    protected  processMessage(msgString: any) {
        let msg = JSON.parse(msgString);
        console.log(" Monitoring LTC address: ", this.address, " Response: ", JSON.stringify(msg));

        let amount = +(+msg.total / 100000000).toFixed(8);
        this.amountDetected.next(amount)
    }
}
