import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../services/data.service';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

    today: any = Date.now();

    constructor(public data: DataService) { }

    ngOnInit() {
    }

}
