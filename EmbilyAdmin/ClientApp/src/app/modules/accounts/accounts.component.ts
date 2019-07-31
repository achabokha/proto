import { Component, ViewChild } from '@angular/core';
import { DataService } from '../../services/data.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountsService } from '../../services/accounts.service';

@Component({
    selector: 'app-accounts',
    templateUrl: './accounts.component.html',
    styleUrls: ['./accounts.component.scss']
})

export class AccountsComponent {

    loadingIndicator: boolean = false;
    selected = [];
    editing = {};
    temp = [];

    constructor(public data: DataService, private accountService: AccountsService, private router: Router, private route: ActivatedRoute) {

    }

    ngOnInit(): void {
        this.getAccounts();
    }

    getAccounts() {
        this.accountService.getAccounts().subscribe((r) => {
            this.data.accounts = r;
            this.temp = r;
        });
    }

    onSelect({ selected }) {
        //console.log('Select Event', selected, this.selected);
        this.router.navigate(["accounts", this.selected[0].accountId]);
    }

    rowUpdate(event, cell, rowIndex) {

        this.editing[rowIndex + '-' + cell] = false;
        this.data.accounts[rowIndex][cell] = event.target.value;
        this.data.accounts = [...this.data.accounts]

        //this.roleService.updateUserRole(this.roles[rowIndex]).subscribe(result => {
        //    console.log('Roles database update successful!');
        //});
    }

    updateFilter(event, columnName) {

        this.data.accounts = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.accounts = temp;
    }
}
