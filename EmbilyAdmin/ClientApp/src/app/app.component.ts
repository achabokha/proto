import { Component, OnInit, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/platform-browser';
import { environment } from 'src/environments/environment';


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    title = 'EmbilyAdmin';
    domain: any;

    constructor(@Inject(DOCUMENT) private document: any) {
    }

    ngOnInit(): void {
        this.domain = this.document.location.hostname;
        console.log('domain name:', this.domain);
    }
}
