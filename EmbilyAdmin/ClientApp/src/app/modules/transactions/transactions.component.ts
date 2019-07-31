import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { NgbTabChangeEvent } from '@ng-bootstrap/ng-bootstrap';
import { DataService } from '../../services/data.service';
import { Transaction } from '../../models';
import { AccountsService } from '../../services/accounts.service';
import { Page } from '../../models/page';

@Component({
    selector: 'app-transactions',
    templateUrl: './transactions.component.html',
    styleUrls: ['./transactions.component.css']
})

export class TransactionsComponent implements OnInit {

    page = new Page();
    public transactions: Transaction[] | null = null;
    loadingIndicator: boolean = true;
    temp = [];

    constructor(public data: DataService, private accountsService: AccountsService, private router: Router, private route: ActivatedRoute) {

    }

    ngOnInit(): void {
        this.page.pageNumber = 0;
        this.page.size = 20;
        this.getData();
    }

    getData() {
        this.transactions = null;
        this.setPage({ offset: 0 });
        //this.accountsService.getTransactions().subscribe(r => {
        //    this.transactions = r;
        //    this.temp = r;
        //});
    }

    accountNumberFilter(event, columnName) {

        this.transactions = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].accountNumber.toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.transactions = temp;
    }

    updateFilter(event, columnName) {

        this.transactions = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.transactions = temp;
    }

    setPage(pageInfo) {
        this.transactions = null;
        this.page.pageNumber = pageInfo.offset;
        this.accountsService.getTransactions(this.page).subscribe(pagedData => {
            this.page = pagedData.page;
            this.transactions = pagedData.pageTransactions;
            this.temp = pagedData.pageTransactions;
        });
    }
}
