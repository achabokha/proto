import { Component, OnInit } from '@angular/core';
import { CheckoutBase } from '../common/checkout-base';
import { ActivatedRoute, Router } from '@angular/router';

import { DataService } from '../../../services/data.service';
import { ApplicationService } from '../../../services/application.service';

@Component({
    selector: 'app-app-submit',
    templateUrl: './submit.component.html',
    styleUrls: ['./submit.component.css']
})
export class SubmitComponent extends CheckoutBase implements OnInit {

    complete: boolean = false;
    spinning: boolean = false;

    rejectedInfo: any;

    state: string = 'submit';

    constructor(private router: Router, route: ActivatedRoute, private applicationService: ApplicationService, private data: DataService) {
        super(route);
    }

    ngOnInit(): void {
        this.applicationService.getComments(this.appId)
            .subscribe((result) => {
                this.rejectedInfo = JSON.parse(result);
            }, error => {
                this.state = 'error';
            });
    }

    back() {
        this.router.navigate(["application", this.currencyCode, "shipping-address", this.appId], { queryParams: this.queryParams });
    }

    submit() {
        this.spinning = true;
        this.applicationService.submit(this.appId)
            .subscribe(() => {
                this.complete = true;
                this.data.clean();
            }, error => {
                this.state = 'error';
            });
    }
}
