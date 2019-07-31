import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { AffiliateService } from '../../../services/affiliate.service';

@Component({
    selector: 'app-tokens',
    templateUrl: './tokens.component.html',
    styleUrls: ['./tokens.component.scss']
})
export class TokensComponent implements OnInit {

    loadingIndicator: boolean = false;

    temp = [];

    constructor(public data: DataService, private affiliateService: AffiliateService) {
    }

    ngOnInit(): void {
        this.affiliateService.getAffiliateTokens().subscribe(t => {
            this.data.affiliateTokens = t.tokenList;
            this.temp = t.tokenList;
        });
    }

    affiliateUserFilter(event, columnName) {

        this.data.affiliateTokens = this.temp;
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

        this.data.affiliateTokens = temp;
    }

    affiliateEmailFilter(event, columnName) {

        this.data.affiliateTokens = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].email.toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.affiliateTokens = temp;
    }

}
