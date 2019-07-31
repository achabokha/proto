import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';

@Component({
    selector: 'app-affiliate',
    templateUrl: './affiliate.component.html',
    styleUrls: ['./affiliate.component.scss']
})
export class AffiliateComponent implements OnInit {

    loadingIndicator: boolean = false;

    constructor(public data: DataService) {
    }

    ngOnInit(): void {
    }

}
