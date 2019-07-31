import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AccountsService } from '../../services/accounts.service';
import { Application, Account } from '../../models';
import { DataService } from '../../services/data.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

    constructor(private router: Router, private authService: AuthService, private accountsService: AccountsService, public data: DataService) { }

    ngOnInit(): void {
        if (!this.data.stage) {
            this.getData();
        }
    }

    getData() {
        this.accountsService.getDashboardInfo().subscribe(info => {
            this.data.stage = info.stage;
            if (this.data.stage == "fresh") {
                this.router.navigate(["/application"]);
            }

            if (info.applications) {
                this.data.applications = info.applications as Application[];
            }

            if (info.accounts) {
                this.data.accounts = info.accounts as Account[];

                this.data.accounts.forEach((a) => {
                    this.accountsService.getBalance(a.accountId).subscribe(balance => {
                        a.balance = balance;
                    });
                });
            }

            this.data.isAffiliate = info.isAffiliate;

            if (info.affiliateAccounts) {
                this.data.affiliateAccounts = info.affiliateAccounts as Account[];

                this.data.affiliateAccounts.forEach((a) => {
                    this.accountsService.getBalance(a.accountId).subscribe(balance => {
                        a.balance = balance;
                    });
                });
            }
        });
    }

    getProduct() {
        this.router.navigate(["/application"]);
    }

    onRefresh() {
        this.getData();
    }
}
