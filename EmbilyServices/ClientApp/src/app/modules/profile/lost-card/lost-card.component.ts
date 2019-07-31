import { Component, OnInit, Input } from '@angular/core';
import { Account } from "../../../models";
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router, ActivatedRoute } from '@angular/router';


import { AccountsService } from '../../../services/accounts.service';
import { DataService } from '../../../services/data.service';
//import { take } from 'rxjs/operator/take';

@Component({
    selector: 'app-lost-card',
    templateUrl: './lost-card.component.html',
    styleUrls: ['./lost-card.component.scss'] 
})

export class LostCardComponent implements OnInit {

    @Input() account: Account;

    lockedCard: any;

    cardNumber: string;

    constructor(public modalService: NgbModal, private accountsService: AccountsService, private data: DataService, private router: Router, private route: ActivatedRoute) {
    }

    ngOnInit(): void {
        this.cardNumber = this.getFormattedCardNumber(this.account.cardNumber);

        if (this.account.accountStatusString == "Suspended") {
            this.lockedCard = true;
        }
    }

    getData() {
        this.account.balance = null;
        this.accountsService.getBalance(this.account.accountId).subscribe(balance => {
            this.account.balance = balance;
        });
    }

    getFormattedCardNumber(number: string) {
        return this.accountsService.getFormattedCardNumber(number);
    }

    setLostCard(flag: boolean) {
        this.accountsService.setLostCard(this.account.accountId).subscribe(lc => {
            this.data.clean();
            this.lockedCard = true;

            if (flag) {
                this.router.navigate(["app/" + lc.newApplication.currencyCodeString + "/terms/", lc.newApplication.applicationId]);
            }
        });
    }
}
