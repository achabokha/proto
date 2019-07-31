import { Component, Inject, OnInit, Input } from '@angular/core';
import { Http } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-processing',
    templateUrl: './processing.component.html',
    styleUrls: ['./processing.component.css']
})
export class ProcessingComponent implements OnInit {

    public state: string = 'none';

    public message: string | undefined;

    @Input() service: any;

    @Input() stateChange: any | undefined;

    constructor() { }

    ngOnInit(): void {
        this.state = 'none';

        this.stateChange.subscribe((stateInfo: any) => {
            this.state = stateInfo.status;
            this.message = stateInfo.message;
        });
    }
}
