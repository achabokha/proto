import { Component, OnInit, Input } from '@angular/core';
import { AccountsService } from '../../../services/accounts.service';
import { DataService } from '../../../services/data.service';
import { Account } from "../../../models";

@Component({
    selector: 'app-card',
    templateUrl: './card.component.html',
    styleUrls: ['./card.component.scss']
})

export class CardComponent implements OnInit {

    @Input() account: Account;

    cardNumber: string;
    spinBalance: boolean = false;

    constructor(private accountsService: AccountsService, private data: DataService) {
    }

    ngOnInit(): void {
        this.cardNumber = this.getFormattedCardNumber(this.account.cardNumber);
        //if (!this.data.cardSpinned) {
        //    this.spinCardNumber();
        //}
    }

    getData() {
        this.spinBalance = true;
        this.account.balance = 0;
        this.accountsService.getBalance(this.account.accountId).subscribe(balance => {
            this.account.balance = balance;
        }, err => {
            }, () => {
                this.spinBalance = false;
        }
        );
    }

    getFormattedCardNumber(number: string) {
        return this.accountsService.getFormattedCardNumber(number);
        //return `${number.slice(0, 4)} ${number.slice(4, 8)} ${number.slice(8, 12)} ${number.slice(12, 16)}`;
    }

    spinCardNumber() {
        let n = this.account.cardNumber;
        let o = [+n[0], +n[1], +n[2], +n[3], +n[4], +n[5], +n[6], +n[7], +n[8], +n[9], +n[10], +n[11], +n[12], +n[13], +n[14], +n[15]];
        let a: Array<number> = new Array<number>(16);

        for (let i = 0; i < 16; i++) {
            a[i] = Math.floor((Math.random() * 10));
        }

        this.restartSpin(o, a, null, 5, 50, 700);
    }

    startSpin(o: Array<number>, a: Array<number>, speed: number, slowdown: boolean): any {
        let spinner = setInterval(() => {
            for (let i = 0; i < 16; i++) {
                a[i] = (slowdown && a[i] == o[i]) ? o[i] : Math.floor((Math.random() * 10));
            }
            this.cardNumber = this.getFormattedCardNumber(a.join(''));
        }, speed);
        return spinner;
    }

    restartSpin(o: Array<number>, a: Array<number>, spinner: any, step: number, speed: number, duration: number) {

        //console.log(`Step: ${step}`);

        if (spinner) clearInterval(spinner);

        if (step < 0) {
            this.cardNumber = this.getFormattedCardNumber(o.join(''));
            this.data.cardSpinned = true;
            return;
        }

        spinner = this.startSpin(o, a, speed, step < 4);

        setTimeout(() => {
            this.restartSpin(o, a, spinner, step - 1, (6 - step) * 50, (6 - step) * 500);
        }, duration);
    }

    ngOnDestroy() {

    }
}
