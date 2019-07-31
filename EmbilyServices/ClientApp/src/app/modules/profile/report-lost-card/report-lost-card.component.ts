import { Component, OnInit, Input } from '@angular/core';
import { Router } from "@angular/router";
import { AccountsService } from '../../../services/accounts.service';
import { DataService } from '../../../services/data.service';
import { Account } from "../../../models";

@Component({
    selector: 'app-report-lost-card',
    templateUrl: './report-lost-card.component.html',
    styleUrls: ['./report-lost-card.component.css']
})

export class ReportLostCardComponent implements OnInit {
    
    
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