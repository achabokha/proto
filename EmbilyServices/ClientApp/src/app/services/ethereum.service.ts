import { Injectable } from '@angular/core';
import { Observable, Subscription, timer } from "rxjs";
import { BlockchainService } from "./blockchain.service";
import { HubConnection } from '@aspnet/signalr';
import * as signalR from '@aspnet/signalr';

@Injectable()
export class EthereumService extends BlockchainService {

    protected coinName: string = "Ethereum";
    protected url: string = "wss://socket.etherscan.io/wshandler";

    private timerSubscription: Subscription | undefined;

    private connection: HubConnection | undefined;

    constructor() {
        super();

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("/hubs/blockchainhub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        let self = this;

        this.connection.on("TransactionDetected", (currencyCode, address, amount) => {
            console.log('message: ', currencyCode, address, amount);
            // amount / 1000000040000000000???
            self.monitorETH(address, amount);

        });

    }

    connectAndMonitor(address: string): void {
        console.log(`ETH: monitoring ${address}`);
        this.address = address;
        this.connection.start();
        this.connected = true;
    }

    close(): void {
        this.connection.stop();
        this.connected = false;
        console.log('ETH: connection stopped');
    }

    monitorETH(address, amount) {
        if (this.address == address) {
            console.log('amount for the address detected!');
            this.amountDetected.next(amount);
        }
    }

    protected monitor() {
    }

    protected processMessage(msgString: any) {
    }


    //eth_sample_response: any =
    //    {
    //        "event": "txlist",
    //        "address": "0xfb488f7e8962e3d273c47f6170372126dae53a7b",
    //        "result": [
    //            {
    //                "blockNumber": "4532055",
    //                "timeStamp": "1510399226",
    //                "hash": "0x68e7ee1d6e55cce798d3a7e75a0637de88d0a8f322b12cbe86cca78822a1d0ee",
    //                "nonce": "0",
    //                "blockHash": "0x311705fb2c81a42bd74ed04b0cb5252f80b861cd2f3f3806d54fdaa891bef000",
    //                "transactionIndex": "7",
    //                "from": "0x06f21955c342a76a25f9e0a8e82168df906b08cc",
    //                "to": "0xfb488f7e8962e3d273c47f6170372126dae53a7b",
    //                "value": "16344140000000000",
    //                "gas": "90000",
    //                "gasPrice": "20000000000",
    //                "input": "0x",
    //                "contractAddress": "",
    //                "cumulativeGasUsed": "188556",
    //                "gasUsed": "21000",
    //                "confirmations": "1"
    //            }
    //        ]
    //    }
}
