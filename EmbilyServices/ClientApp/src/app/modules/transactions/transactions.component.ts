import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../services/data.service';
import { AccountsService } from '../../services/accounts.service';
import { Transaction } from '../../models';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { NgbTabChangeEvent } from '@ng-bootstrap/ng-bootstrap';
import { AllComponent } from './all/all.component';

@Component({
    selector: 'app-transactions',
    templateUrl: './transactions.component.html',
    styleUrls: ['./transactions.component.css']
})

export class TransactionsComponent implements OnInit {

    @ViewChild('t') public t: any;
    
    isAffiliate: any;

    constructor(public data: DataService, private accountsService: AccountsService, private router: Router, private route: ActivatedRoute) {

    }

    ngOnInit(): void {
        this.accountsService.IsRoleAffiliate().subscribe((r) => this.isAffiliate = this.data.isAffiliate = r.isAffiliate);

        if (!this.data.affiliateTransactions) {
            this.accountsService.getAffiliateTransactions().subscribe((r) => {
                this.data.affiliateTransactions = r;
            });
        }
        if (!this.data.loadTransactions) {
            this.accountsService.getLoadTransactions().subscribe((r: Transaction[]) => {
                this.data.loadTransactions = r;
            });
        }
        if (!this.data.allTransactions) {
            this.accountsService.getAllTransactions().subscribe((r: any) => {
                this.data.allTransactions = r;
            }, err => {
                this.data.allTransactions = [];
            });
        }

        if (!this.route.firstChild) {
            this.router.navigate(['transactions', 'loads']);
        } else {
            this.t.activeId = this.route.firstChild.routeConfig.path;
        }
    }

    getData() {
        this.data.cleanTransactions();

        this.accountsService.getAffiliateTransactions().subscribe((r) => {
            this.data.affiliateTransactions = r;
        });

        this.accountsService.getLoadTransactions().subscribe((r: Transaction[]) => {
            this.data.loadTransactions = r;
        });

        this.accountsService.getAllTransactions().subscribe((r: Transaction[]) => {
            this.data.allTransactions = r;
        }, err => {
            this.data.allTransactions = [];
        });
    }

    onTabChange($event: NgbTabChangeEvent) {
        this.router.navigate([`/transactions/${$event.nextId}`]);
    }
}
