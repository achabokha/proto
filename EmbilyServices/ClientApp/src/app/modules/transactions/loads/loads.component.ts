import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { Transaction } from '../../../models';
import { AccountsService } from '../../../services/accounts.service';

@Component({
    selector: 'app-loads',
    templateUrl: './loads.component.html',
    styleUrls: ['./loads.component.css']
})
export class LoadsComponent implements OnInit {

    loadingIndicator: boolean = false;
    temp = [];

    constructor(public data: DataService, private accountsService: AccountsService) {
    }

    ngOnInit(): void {
        if (!this.data.loadTransactions) {
            this.accountsService.getLoadTransactions().subscribe((r: Transaction[]) => {
                this.temp = r;
            });
        }       
    }

    updateFilter(event, columnName) {

        this.data.loadTransactions = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function(d) {
            if (d[columnName]) {
                return d[columnName].toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.loadTransactions = temp;
    }
}
