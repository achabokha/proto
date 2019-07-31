import { Injectable } from '@angular/core';
import { Subject } from "rxjs";


export abstract class BlockchainService {

    protected address: string | undefined;

    protected abstract url: string;
    protected abstract coinName: string;
    protected socket: any;

    amountDetected: Subject<number> = new Subject<number>();

    connected: boolean = false;
   
    getCoinName(): string {
        return this.coinName;
    }

    connectAndMonitor(address: string): void {
        if (typeof WebSocket !== 'undefined') {
            this.connected = false;

            this.address = address;

            this.socket = new WebSocket(this.url)

            console.log('connecting to ', this.url);

            this.socket.onmessage = (r: any) => {
                this.processMessage(r.data);
            };

            this.socket.onerror = () => {
                console.log('error in the socket');
            }

            this.socket.onopen = () => {
                this.monitor();
                this.connected = true;
            }
        }
    }

    protected abstract monitor(): void;

    protected abstract processMessage(msgString: any): void;

    close(): void {
        console.log("Closing...");
        if (this.socket) {
            this.socket.close();
            this.connected = false;
            console.log("Closing socket...")
        }
    }

}
