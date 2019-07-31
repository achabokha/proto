import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../../services/data.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AffiliateService } from '../../../services/affiliate.service';

@Component({
    selector: 'app-affiliate',
    templateUrl: './affiliate.component.html',
    styleUrls: ['./affiliate.component.scss']
})
export class AffiliateComponent implements OnInit {

    loadingIndicator: boolean = false;
    selected = [];

    temp = [];

    constructor(public data: DataService, private affiliateService: AffiliateService, private router: Router, private route: ActivatedRoute) {
    }

    ngOnInit(): void {
            this.affiliateService.getAffiliateUsers().subscribe(t => {
                this.data.affiliateUsers = t.users;
                this.temp = t.users;
            });
    }

    onSelect({ selected }) {
        //console.log("Click", this.selected[0].id)
        this.router.navigate(["users/details", this.selected[0].id]);
    }

    updateFilter(event, columnName) {
        this.data.affiliateUsers = this.temp;
        const val = event.target.value.toLowerCase();

        if (val == '') {
            return;
        }

        const temp = this.temp.filter(function (d) {
            if (d[columnName]) {
                return d[columnName].toString().toLowerCase().indexOf(val) !== -1 || !val;
            }
        });

        this.data.affiliateUsers = temp;
    }
}
