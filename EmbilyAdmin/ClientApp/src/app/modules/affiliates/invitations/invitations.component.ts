import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { AffiliateService } from '../../../services/affiliate.service';

@Component({
    selector: 'app-invitations',
    templateUrl: './invitations.component.html',
    styleUrls: ['./invitations.component.css']
})
export class InvitationsComponent implements OnInit {

    loadingIndicator: boolean = false;

    temp = [];

    constructor(public data: DataService, private affiliateService: AffiliateService) {
    }

    ngOnInit(): void {
            this.affiliateService.getAffiliateInvite().subscribe(al => {
                this.data.affiliateInvite = al.affiliateEmails;
                this.temp = al.affiliateEmails;
            });
    }

    updateFilter(event, columnName) {
        this.data.affiliateInvite = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.affiliateInvite = temp;
    }

    affiliateUserFilter(event, columnName) {

        this.data.affiliateInvite = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                var fullName = d[columnName].firstName + d[columnName].lastName;
                return fullName.toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.affiliateInvite = temp;
    }

    affiliateEmailFilter(event, columnName) {

        this.data.affiliateInvite = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].email.toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.affiliateInvite = temp;
    }
}
