import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../services/data.service';
import { Transaction } from '../../models';
import { Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { NgbTabChangeEvent } from '@ng-bootstrap/ng-bootstrap';
import { AffiliateService } from '../../services/affiliate.service';

@Component({
    selector: 'app-affiliates',
    templateUrl: './affiliates.component.html',
    styleUrls: ['./affiliates.component.css']
})

export class AffiliatesComponent implements OnInit {

    @ViewChild('t') public t: any;

    constructor(public data: DataService, private affiliateService: AffiliateService, private router: Router, private route: ActivatedRoute) {

    }

    ngOnInit(): void {

        //if (!this.data.affiliateInvite) { 
        //    this.affiliateService.getAffiliateInvite().subscribe(al => {
        //        this.data.affiliateInvite = al.affiliateEmails;
        //    });
        //}
        //if (!this.data.affiliateUsers) {
        //    this.affiliateService.getAffiliateUsers().subscribe(t => {
        //        this.data.affiliateUsers = t.users;
        //    });
        //}
        //if (!this.data.affiliateTokens) {
        //    this.affiliateService.getAffiliateTokens().subscribe(t => {
        //        this.data.affiliateTokens = t.tokenList;
        //    });
        //}

        if (!this.route.firstChild) {
            this.router.navigate(['affiliates', 'affiliatesUser']);
        } else {
            this.t.activeId = this.route.firstChild.routeConfig.path;
        }
    }

    getData() {
        this.data.cleanTransactions();

        this.affiliateService.getAffiliateInvite().subscribe(al => {
            this.data.affiliateInvite = al.affiliateEmails;
        });

        this.affiliateService.getAffiliateUsers().subscribe(t => {
            this.data.affiliateUsers = t.users;
        });

        this.affiliateService.getAffiliateTokens().subscribe(t => {
            this.data.affiliateTokens = t.tokenList;
        });
    }

    onTabChange($event: NgbTabChangeEvent) {
        this.router.navigate([`/affiliates/${$event.nextId}`]);
    }
}
