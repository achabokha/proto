import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountsService } from '../../../services/accounts.service';
import { DataService } from '../../../services/data.service';
import { Account } from "../../../models";

@Component({
    selector: 'app-setpin-parent',
    templateUrl: './setpin-parent.component.html',
    styleUrls: ['./setpin-parent.component.scss']
})

export class SetpinParentComponent implements OnInit {


    constructor(private router: Router, private accountsService: AccountsService, public data: DataService) {

    }

    ngOnInit(): void {
        this.getData();
    }

    getData() {
        this.accountsService.getCardAccounts().subscribe(info => {
            if (info.accounts) {
                this.data.accounts = info.accounts as Account[];

                this.data.accounts.forEach((a) => {
                    this.accountsService.getBalance(a.accountId).subscribe(balance => {
                        a.balance = balance;
                    });
                });
            }
        });
    }
}
