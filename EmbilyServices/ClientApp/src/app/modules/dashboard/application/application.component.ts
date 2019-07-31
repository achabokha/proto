import { Component, Input, OnInit } from '@angular/core';

@Component({
    selector: 'app-application',
    templateUrl: './application.component.html',
    styleUrls: ['./application.component.scss']
})

export class ApplicationComponent implements OnInit {

    @Input() app: any;

    rejectedInfo: any;

    constructor() {
    }

    ngOnInit(): void {
        if (this.app.comments) this.rejectedInfo = JSON.parse(this.app.comments);
    }
}
