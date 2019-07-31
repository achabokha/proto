import { ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ProgressIndicatorComponent } from './../progress-indicator/progress-indicator.component';

export class CheckoutBase {
    public appId: string;
    public currencyCode: string;
    public resubmit: boolean = false;
    
    public queryParams: any;

    constructor(private route: ActivatedRoute) {

        // no need for error check routing map will check for parameters --
        this.appId = this.route.snapshot.paramMap.get('appId') || "start";
        this.currencyCode = this.route.snapshot.paramMap.get('currencyCode') || "";
        this.resubmit = !!this.route.snapshot.queryParamMap.get('resubmit') || false;
        this.queryParams = this.resubmit? { resubmit: this.resubmit } : null;
        //this.currencyCode = this.route.snapshot.queryParamMap.get('currencyCode') || "USD";
    }
}
