import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'app-progress-indicator',
    templateUrl: './progress-indicator.component.html',
    styleUrls: ['./progress-indicator.component.scss']
})
export class ProgressIndicatorComponent implements OnInit {

    @Input('step') stepNumber: number = 0;

    @Input() resubmit: boolean = false;

    constructor() { }

    ngOnInit() {
    }

}
